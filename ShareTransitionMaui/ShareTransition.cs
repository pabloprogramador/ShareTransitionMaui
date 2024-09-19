using System;
using System.Threading;
using System.Threading.Tasks;



namespace ShareTransitionMaui
{
    public class ShareTransition : Grid
    {

        public bool IsBusy;
        public int Current = 0;

        private int zindex;
        private List<PageRoot> _roots;


        public ShareTransition()
        {
            this.IgnoreSafeArea = true;
            this.IsClippedToBounds = false;
            _roots = new List<PageRoot>();
        }

        protected async override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            await Task.Delay(200);

            foreach (IView item in this.Children)
            {
                var list = SearchAllViews(new PageView { Item = item });
                list.RemoveAt(0);
                _roots.Add(new PageRoot
                {
                    Root = item,
                    Views = list
                });
                ((VisualElement)item).IsVisible = false;
            }
            zindex = _roots.Count;
            if (Current >= 0 && Current < _roots.Count)
                ((VisualElement)_roots[Current].Root).IsVisible = true;

        }

        public async Task GoTo(int index)
        {
            if (IsBusy) return;
            IsBusy = true;

            ((VisualElement)_roots[index].Root).ZIndex = zindex;
            zindex++;

            var list = FindAllClassIds((VisualElement)_roots[index].Root);

            ((VisualElement)_roots[index].Root).IsVisible = true;

            foreach (var item in list)
            {

                var currentObj = FindByClassId<Image>((Element)_roots[Current].Root, item);
                var nextObj = FindByClassId<Image>((Element)_roots[index].Root, item);
                if (nextObj != null)
                {
                    nextObj.Opacity = 0;

                    var temp = await CloneElement(currentObj, this);
                    
                    //temp.Opacity = .5;
                    //temp.BackgroundColor = Colors.Yellow;
                    //temp.VerticalOptions = LayoutOptions.Start;
                    //temp.HorizontalOptions = LayoutOptions.Start;
                    if (currentObj.Width / currentObj.Height > nextObj.Width/nextObj.Height)
                    temp.Aspect = Aspect.AspectFill;

                    temp.InputTransparent = true;
                    temp.ZIndex = zindex;
                    nextObj.Source = currentObj.Source;
                    zindex++;

                    var currentPoint = currentObj.GetAbsolutePosition();

                    var nextPoint = nextObj.GetAbsolutePosition();



                    AnimateImage(temp,
                        currentPoint.X, currentPoint.Y, nextPoint.X, nextPoint.Y,
                        currentObj.Rotation,
                        nextObj.Rotation,
                        currentObj.Width, nextObj.Width,
                        currentObj.Height, nextObj.Height,
                        700, Easing.SpringOut,
                        async () => {
                            this.Children.Remove(temp);
                            nextObj.Opacity = 1;
                        });
                    currentObj.Opacity = 0;
                }
            };

            //foreach (var item in _roots[Current].Views)
            //{
            //    //((VisualElement)item.Item).FadeTo(0, 300);
            //}

            await Task.Delay(300);

            //foreach (var item in _roots[Current].Views)
            //{
            //    ((VisualElement)item.Item).Opacity = 1;
            //}

            ((VisualElement)_roots[Current].Root).IsVisible = false;
            Current = index;
            
            IsBusy = false;
        }

        //private IView SearchByClassId(string classId, List<PageView> views)
        //{
        //    return views.Where(item => ((Element)item.Item).ClassId == classId)?.First()?.Item;   
        //}
       

        private List<PageView> SearchAllViews(PageView view)
        {

            var views = new List<PageView>
            {
                // Adiciona a própria view à lista
                view
            };

            // Se a view for um container (por exemplo, Layout), itera sobre seus filhos
            if (view.Item is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    // Chama o método recursivamente para cada filho
                    views.AddRange(SearchAllViews(new PageView { Item = child }));
                }
            }
            // Se a view for um ContentView, verifica se tem conteúdo
            else if (view.Item is ContentView contentView && contentView.Content is IView content)
            {
                // Chama o método recursivamente para o conteúdo
                views.AddRange(SearchAllViews(new PageView { Item = content }));
            }

            return views;
        }

        private T FindByClassId<T>(Element parent, string classId) where T : Element
        {
            if (parent.ClassId == classId && parent is T)
            {
                return (T)parent;
            }

            foreach (var child in (parent as IVisualTreeElement).GetVisualChildren())
            {
                var result = FindByClassId<T>((Element)child, classId);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private List<string> FindAllClassIds(Element parent)
        {
            List<string> classIds = new List<string>();

            // Se o elemento atual tiver um ClassId, adiciona na lista
            if (!string.IsNullOrEmpty(parent.ClassId))
            {
                classIds.Add(parent.ClassId);
            }

            // Percorre os filhos visuais do elemento
            foreach (var child in (parent as IVisualTreeElement).GetVisualChildren())
            {
                classIds.AddRange(FindAllClassIds((Element)child)); // Recursão para buscar nos filhos
            }

            return classIds;
        }

        private async Task<Image> CloneElement(VisualElement element, Grid targetGrid)
        {
            // Captura o visual do elemento
            var screenshot = await element.CaptureAsync();

            if (screenshot != null)
            {
                // Cria um Image a partir do resultado da captura
                var image = new Image
                {
                    Source = ImageSource.FromStream(() => screenshot.OpenReadAsync().Result),
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = element.Width,   // Ajusta a largura
                    HeightRequest = element.Height,  // Ajusta a altura
                    Opacity = 0
                };

                // Adiciona a imagem à grid
                targetGrid.Children.Add(image);
                await Task.Delay(200);
                return image;
            }
            return null;
        }

        private void AnimateImage(
        VisualElement element,
        double startX, double startY,
        double endX, double endY,
        double startRotation, double endRotation,
        double startWidth, double endWidth,
        double startHeight, double endHeight,
        uint duration,
        Easing easingType,
        Action onCompleted)
        {
            element.Opacity = 1;
            element.TranslationX = startX;
            element.TranslationY = startY;
            element.Rotation = startRotation;
            element.WidthRequest = startWidth;
            element.HeightRequest = startHeight;
            
            element.Animate("CustomAnimation", new Animation(v =>
            {
                element.TranslationX = startX + (endX - startX) * v;
                element.TranslationY = startY + (endY - startY) * v;

                element.Rotation = startRotation + (endRotation - startRotation) * v;

                element.WidthRequest = startWidth + (endWidth - startWidth) * v;
                element.HeightRequest = startHeight + (endHeight - startHeight) * v;

            }), 16, length: duration, easing: easingType, (d, v) =>
            {
                onCompleted?.Invoke();
            });
        }
    }

    public class PageRoot
    {
        public IView Root;
        public List<PageView> Views;
    }

    public class PageView
    {
        public IView Next;
        public IView Item;
        public double PosX;
        public double PosY;
        public int Opacity;
        public double Width;
        public double Height;
        public string Color;

    }

}


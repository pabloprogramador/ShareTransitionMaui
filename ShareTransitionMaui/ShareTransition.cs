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
                    //obj.BackgroundColor = Colors.Green;
                    var temp = await CloneElement(currentObj, this);
                    currentObj.IsVisible = false;
                    temp.InputTransparent = true;
                    temp.ZIndex = zindex;
                    zindex++;
                    nextObj.Source = currentObj.Source;
                    nextObj.IsVisible = false;
                    await temp.ScaleTo(2.2, 700, easing: Easing.SpringOut);
                    this.Children.Remove(temp);
                    nextObj.IsVisible = true;

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

        public async Task<Image> CloneElement(VisualElement element, Grid targetGrid)
        {
            // Captura o visual do elemento
            var screenshot = await element.CaptureAsync();

            if (screenshot != null)
            {
                // Cria um Image a partir do resultado da captura
                var image = new Image
                {
                    Source = ImageSource.FromStream(() => screenshot.OpenReadAsync().Result),
                    WidthRequest = element.Width,   // Ajusta a largura
                    HeightRequest = element.Height  // Ajusta a altura
                };

                // Adiciona a imagem à grid
                targetGrid.Children.Add(image);
                return image;
            }
            return null;
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


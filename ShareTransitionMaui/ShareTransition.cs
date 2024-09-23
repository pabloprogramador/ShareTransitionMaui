using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;

namespace ShareTransitionMaui
{
    public class ShareTransition : Grid
    {

        public bool IsBusy;
        public int Current = 0;

        private int zindex;
        private List<PageRoot> _roots;
        private List<Element> NoBackground;


        public ShareTransition()
        {
            this.IgnoreSafeArea = true;
            this.IsClippedToBounds = false;
            _roots = new List<PageRoot>();
            NoBackground = new List<Element>();
        }

        protected async override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            await Task.Delay(200);

            foreach (VisualElement item in this.Children)
            {
                var list = SearchAllViews(item);
                list.RemoveAt(0);
                _roots.Add(new PageRoot
                {
                    Root = item,
                    Views = list
                });
                item.IsVisible = false;
            }
            zindex = _roots.Count;
            if (Current >= 0 && Current < _roots.Count)
                ((VisualElement)_roots[Current].Root).IsVisible = true;

        }

        public async Task GoTo(int index)
        {
            if (IsBusy) return;
            IsBusy = true;

            var listWithClassId = new List<Element>();

            ((VisualElement)_roots[index].Root).ZIndex = zindex;
            zindex++;

            var list = FindAllClassIds((VisualElement)_roots[index].Root);

            ((VisualElement)_roots[index].Root).IsVisible = true;

            foreach (var item in _roots[index].Views)
            {
                item.Opacity = 0;
            }

            //IMAGE
            foreach (var item in list)
            {

                var currentObj = FindByClassId<Image>(_roots[Current].Root, item);
                var nextObj = FindByClassId<Image>(_roots[index].Root, item);
                if (nextObj != null)
                {
                    listWithClassId.Add(currentObj);
                    listWithClassId.Add(nextObj);

                    nextObj.Opacity = 0;

                    var temp = await CloneImage(currentObj, this);
                    
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
            }

            //SHAPE
            foreach (var item in list)
            {
                var currentObj = FindByClassId<Shape>(_roots[Current].Root, item);
                var nextObj = FindByClassId<Shape>(_roots[index].Root, item);
                if (nextObj != null)
                {
                    listWithClassId.Add(currentObj);
                    listWithClassId.Add(nextObj);

                    nextObj.Opacity = 0;

                    var temp = CloneShape(currentObj);
                    //temp.VerticalOptions = LayoutOptions.Center;
                    //temp.HorizontalOptions = LayoutOptions.Center;
                    temp.ClassId = "";
                    temp.InputTransparent = true;
                    
                    this.Children.Add(temp);
                    currentObj.Opacity = 0;
                    //temp.ZIndex = zindex;
                    //zindex++;

                    ShapeAnimationExtensions.AnimateShapeAsync(temp, currentObj, nextObj, 700, Easing.Linear,
                            async () =>
                            {
                                this.Children.Remove(temp);
                                if(nextObj.Fill == null)
                                {
                                   NoBackground.Add(nextObj);
                                   CopyColorShape(currentObj, nextObj);
                                }

                                if (NoBackground.Contains(currentObj))
                                {
                                    currentObj.Fill = null;
                                }
                                nextObj.Opacity = 1;
                            }
                    );

                    
                }
            }

            foreach (var item in _roots[index].Views)
            {
                if (!listWithClassId.Contains(item))
                {
                    item.FadeTo(1, 300);
                }
            }

            foreach (var item in _roots[Current].Views)
            {
                if(!listWithClassId.Contains(item))
                item.FadeTo(0, 300);
            }

            await Task.Delay(700);

            foreach (var item in _roots[Current].Views)
            {
                item.Opacity = 1;
            }


            _roots[Current].Root.IsVisible = false;

            Current = index;
            
            IsBusy = false;
        }

        private List<VisualElement> SearchAllViews(VisualElement view)
        {

            var views = new List<VisualElement>
            {
                // Adiciona a própria view à lista
                view
            };

            // Se a view for um container (por exemplo, Layout), itera sobre seus filhos
            if (view is Layout layout)
            {
                foreach (var child in layout.Children)
                {
                    // Chama o método recursivamente para cada filho
                    views.AddRange(SearchAllViews((VisualElement)child));
                }
            }
            // Se a view for um ContentView, verifica se tem conteúdo
            else if (view is ContentView contentView && contentView.Content is IView content)
            {
                // Chama o método recursivamente para o conteúdo
                views.AddRange(SearchAllViews((VisualElement)content));
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

        private static Shape CloneShape(Shape shape)
        {
            var source = (RoundRectangle)shape;
            
            var clonedShape = new RoundRectangle();

            clonedShape.Stroke = source.Stroke;
            clonedShape.StrokeThickness = source.StrokeThickness;
            clonedShape.CornerRadius = source.CornerRadius;
            clonedShape.StrokeDashArray = source.StrokeDashArray;
            clonedShape.StrokeDashOffset = source.StrokeDashOffset;
            clonedShape.StrokeLineCap = source.StrokeLineCap;
            clonedShape.StrokeLineJoin = source.StrokeLineJoin;
            clonedShape.StrokeMiterLimit = source.StrokeMiterLimit;
            
            clonedShape.HorizontalOptions = LayoutOptions.Start;
            clonedShape.VerticalOptions = LayoutOptions.Start;


            CopyColorShape(source, clonedShape);

            return clonedShape;
        }

        private static void CopyColorShape(Shape source, Shape target)
        {
            if (source.Fill is LinearGradientBrush linearGradient)
            {
                // Clonando o gradiente linear
                var gradientClone = new LinearGradientBrush
                {
                    StartPoint = linearGradient.StartPoint,
                    EndPoint = linearGradient.EndPoint
                };

                foreach (var gradientStop in linearGradient.GradientStops)
                {
                    gradientClone.GradientStops.Add(new GradientStop
                    {
                        Offset = gradientStop.Offset,
                        Color = gradientStop.Color
                    });
                }

                target.Fill = gradientClone;
            }
            else if (source.Fill is RadialGradientBrush radialGradient)
            {
                // Clonando o gradiente radial
                var gradientClone = new RadialGradientBrush
                {
                    Center = radialGradient.Center,
                    Radius = radialGradient.Radius,
                    GradientStops = radialGradient.GradientStops
                };

                foreach (var gradientStop in radialGradient.GradientStops)
                {
                    gradientClone.GradientStops.Add(new GradientStop
                    {
                        Offset = gradientStop.Offset,
                        Color = gradientStop.Color
                    });
                }

                target.Fill = gradientClone;
            }
        }

        private async Task<Image> CloneImage(VisualElement element, Layout targetGrid)
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
        public VisualElement Root;
        public List<VisualElement> Views;
    }

}


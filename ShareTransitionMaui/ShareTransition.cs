using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls;

namespace ShareTransitionMaui
{
    public class ShareTransition : Grid
    {

        public int LabelDuration { get; set; } = 250;
        public Easing LabelEasing { get; set; } = Easing.Linear;

        public int ImageDuration { get; set; } = 250;
        public Easing ImageEasing { get; set; } = Easing.Linear;

        public int GridDuration { get; set; } = 250;
        public Easing GridEasing { get; set; } = Easing.Linear;

        public int ShapeDuration { get; set; } = 250;
        public Easing ShapeEasing { get; set; } = Easing.Linear;

        public int FadeDuration { get; set; } = 250;

        public int Current = 0;

        private bool IsBusy;
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

            foreach (VisualElement item in this.Children)
            {
                if (item != this.Children[0])
                    item.Opacity = 0;
            }

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

            foreach (VisualElement item in this.Children)
            {
                item.Opacity = 1;
            }

        }

        public async Task GoTo(int index)
        {
            bool hasImage = false;
            bool hasShape = false;
            bool hasLabel = false;
            bool hasGrid = false;

            if (IsBusy) return;
            IsBusy = true;

            var listWithClassId = new List<Element>();

            _roots[Current].Root.ZIndex = zindex;
            zindex++;

            var list = FindAllClassIds(_roots[index].Root);

            _roots[index].Root.IsVisible = true;

            _roots[index].Root.Opacity = 0;

            //IMAGE
            foreach (var item in list)
            {

                var currentObj = FindByClassId<Image>(_roots[Current].Root, item);
                var nextObj = FindByClassId<Image>(_roots[index].Root, item);
                if (nextObj != null)
                {
                    hasImage = true;
                    listWithClassId.Add(currentObj);
                    listWithClassId.Add(nextObj);

                    nextObj.Opacity = 0;

                    var temp = await CloneImage(currentObj, this);

                    //if (currentObj.Width / currentObj.Height > nextObj.Width / nextObj.Height)
                    //    temp.Aspect = Aspect.AspectFill;

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
                        (uint)ImageDuration, ImageEasing,
                        async () =>
                        {
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
                    hasShape = true;
                    listWithClassId.Add(currentObj);
                    listWithClassId.Add(nextObj);

                    nextObj.Opacity = 0;

                    var temp = CloneShape(currentObj);
                    if (temp != null)
                    {
                        temp.ClassId = "";
                        temp.InputTransparent = true;
                        temp.ZIndex = -1;

                        this.Children.Add(temp);
                        currentObj.Opacity = 0;

                        ShapeAnimation.AnimateShapeAsync(temp, currentObj, nextObj, (uint)ShapeDuration, ShapeEasing,
                                async () =>
                                {
                                    this.Children.Remove(temp);
                                    if (nextObj.Fill == null)
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
            }


            //LABEL
            foreach (var item in list)
            {

                var currentObj = FindByClassId<Label>(_roots[Current].Root, item);
                var nextObj = FindByClassId<Label>(_roots[index].Root, item);
                if (nextObj != null)
                {
                    hasLabel = true;
                    listWithClassId.Add(currentObj);
                    listWithClassId.Add(nextObj);


                    var temp = CloneLabel(currentObj);
                    temp.ClassId = null;
                    temp.InputTransparent = true;

                    this.Children.Add(temp);
                    currentObj.Opacity = 0;
                    nextObj.Text = currentObj.Text;
                    temp.ZIndex = zindex;
                    zindex++;
                    nextObj.Opacity = 0;


                    LabelAnimation.AnimateLabelAsync(temp, currentObj, nextObj, (uint)LabelDuration, LabelEasing,
                            async () =>
                            {
                                this.Children.Remove(temp);
                                nextObj.Opacity = 1;
                            }
                    );
                }
            }


            //_roots[index].Root.Opacity = 1;
           

            _roots[index].Root.FadeTo(1, (uint)FadeDuration);
            _roots[Current].Root.FadeTo(0, (uint)FadeDuration);

            //foreach (var item in _roots[index].Views)
            //{
            //    if (!listWithClassId.Contains(item))
            //    {
            //        item.FadeTo(1, (uint)FadeDuration);
            //    }
            //}

            //foreach (var item in _roots[Current].Views)
            //{
            //    if (!listWithClassId.Contains(item))
            //    {
            //        item.FadeTo(0, (uint)FadeDuration);
            //    }
            //}


            List<int> durationTemp = new List<int>
            {
                FadeDuration
            };

            if (hasLabel)
                durationTemp.Add(LabelDuration);

            if (hasGrid)
                durationTemp.Add(GridDuration);

            if (hasImage)
                durationTemp.Add(ImageDuration);

            if (hasGrid)
                durationTemp.Add(GridDuration);

            await Task.Delay(durationTemp.Max());

            //foreach (var item in _roots[Current].Views)
            //{
            //    item.Opacity = 1;
            //}


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

        private void SetOpacityClassIds(View parent)
        {

            // Se o elemento atual tiver um ClassId, adiciona na lista
            if (!string.IsNullOrEmpty(parent.ClassId))
            {
                parent.Opacity = 0;
            }

            // Percorre os filhos visuais do elemento
            foreach (var child in (parent as IVisualTreeElement).GetVisualChildren())
            {
                FindAllClassIds((Element)child);
            }

            return;
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
            if (shape is Rectangle source)
            {
                var clonedShape = new Rectangle();

                clonedShape.Stroke = source.Stroke;
                if (source.Fill is not LinearGradientBrush)
                    clonedShape.Fill = source.Fill;
                clonedShape.StrokeThickness = source.StrokeThickness;
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

            if (shape is Ellipse sourceEllipse)
            {
                var clonedShape = new Ellipse();

                clonedShape.Stroke = sourceEllipse.Stroke;
                if (sourceEllipse.Fill is not LinearGradientBrush)
                    clonedShape.Fill = sourceEllipse.Fill;
                clonedShape.StrokeThickness = sourceEllipse.StrokeThickness;
                clonedShape.StrokeDashArray = sourceEllipse.StrokeDashArray;
                clonedShape.StrokeDashOffset = sourceEllipse.StrokeDashOffset;
                clonedShape.StrokeLineCap = sourceEllipse.StrokeLineCap;
                clonedShape.StrokeLineJoin = sourceEllipse.StrokeLineJoin;
                clonedShape.StrokeMiterLimit = sourceEllipse.StrokeMiterLimit;

                clonedShape.HorizontalOptions = LayoutOptions.Start;
                clonedShape.VerticalOptions = LayoutOptions.Start;

                CopyColorShape(sourceEllipse, clonedShape);

                return clonedShape;
            }

            if (shape is RoundRectangle sourceRound)
            {
                var clonedShape = new RoundRectangle();

                clonedShape.Stroke = sourceRound.Stroke;
                if (sourceRound.Fill is not LinearGradientBrush)
                    clonedShape.Fill = sourceRound.Fill;
                clonedShape.StrokeThickness = sourceRound.StrokeThickness;
                clonedShape.StrokeDashArray = sourceRound.StrokeDashArray;
                clonedShape.StrokeDashOffset = sourceRound.StrokeDashOffset;
                clonedShape.StrokeLineCap = sourceRound.StrokeLineCap;
                clonedShape.StrokeLineJoin = sourceRound.StrokeLineJoin;
                clonedShape.StrokeMiterLimit = sourceRound.StrokeMiterLimit;

                clonedShape.HorizontalOptions = LayoutOptions.Start;
                clonedShape.VerticalOptions = LayoutOptions.Start;

                clonedShape.CornerRadius = sourceRound.CornerRadius;

                CopyColorShape(sourceRound, clonedShape);

                return clonedShape;
            }

            return null;
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

        private static Label CloneLabel(Label source)
        {
            Label clone = new Label();

            clone.Text = source.Text;
            clone.TextColor = source.TextColor;
            clone.FontSize = source.FontSize;
            clone.FormattedText = source.FormattedText;
            clone.FontAutoScalingEnabled = source.FontAutoScalingEnabled;
            clone.LineBreakMode = source.LineBreakMode;
            clone.LineHeight = source.LineHeight;
            clone.Padding = source.Padding;
            clone.BackgroundColor = source.BackgroundColor;
            clone.MaxLines = source.MaxLines;
            clone.Opacity = source.Opacity;
            clone.FontFamily = source.FontFamily;
            clone.FontAttributes = source.FontAttributes;
            clone.HorizontalTextAlignment = source.HorizontalTextAlignment;
            clone.VerticalTextAlignment = source.VerticalTextAlignment;
            clone.WidthRequest = source.WidthRequest;
            clone.HeightRequest = source.HeightRequest;
            clone.Shadow = source.Shadow;
            clone.HorizontalOptions = LayoutOptions.Start;
            clone.VerticalOptions = LayoutOptions.Start;

            return clone;
        }

        private async Task<Image> CloneImage(Image element, Layout targetGrid)
        {
            var screenshot = await element.CaptureAsync();

            if (screenshot != null)
            {

                //var stream = await screenshot.OpenReadAsync();
                var image = new Image
                {
                    //Source = ImageSource.FromStream(() => stream),
                    Source = element.Source,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    WidthRequest = element.Width,   // Ajusta a largura
                    HeightRequest = element.Height,  // Ajusta a altura
                    Opacity = 0
                };

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


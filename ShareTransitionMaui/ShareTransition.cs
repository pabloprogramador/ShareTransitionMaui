using System;
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
                _roots.Add(new PageRoot
                {
                    Root = item,
                    Views = SearchAllViews(item)
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

            ((VisualElement)_roots[index].Root).Opacity = 0;
            ((VisualElement)_roots[index].Root).IsVisible = true;
            

            ((VisualElement)_roots[Current].Root).FadeTo(0, 300);
            ((VisualElement)_roots[index].Root).FadeTo(1, 300);

            await Task.Delay(300);

            ((VisualElement)_roots[Current].Root).Opacity = 1;
            ((VisualElement)_roots[Current].Root).IsVisible = false;
            Current = index;

            IsBusy = false;
        }

        private Element SearchByClassId(string classId, List<Element> views)
        {
            return views.Where(item => item.ClassId == classId).First();
        }

            private List<IView> SearchAllViews(IView view)
        {
            var views = new List<IView>
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
                    views.AddRange(SearchAllViews(child));
                }
            }
            // Se a view for um ContentView, verifica se tem conteúdo
            else if (view is ContentView contentView && contentView.Content is IView content)
            {
                // Chama o método recursivamente para o conteúdo
                views.AddRange(SearchAllViews(content));
            }

            return views;
        }

    }

    public class PageRoot
    {
        public IView Root;
        public List<IView> Views;
    }


}


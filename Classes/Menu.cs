using Main;

namespace Classes
{
    class Menu
    {
        public List<MenuItem> Actions { get; set; } = new();
        string? Nom { get; set; }
        public MenuItem? DisplayedItem => Actions.Where(action => { return action.IsExecuted; }).Count() > 0 ? Actions.Where(action => { return action.IsExecuted; }).First() : null;
        public Menu(string nom, List<MenuItem> m)
        {
            Actions = m;
        }
        public void Show()
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                Actions[i].Display();
            }
        }
    }

    class MenuItem
    {
        public string Name { get; set; }

        public Action action;

        public Shape Shape { get; set; }

        public bool IsExecuted = false;

        public MenuItem(string name, Action action, Shape shape)
        {
            Name = name;
            this.action = action;
            Shape = shape;
        }

        /*public virtual bool IsClicked() 
        {
            Rect shape = new Rect(0,0,0,0);

            if (Shape is Rect) { shape = (Rect)Shape; return shape.Hover() && Program.MouseLeftPressed(); }
            if (Shape is Circle) { return false; }
            return false;
        }*/

        public virtual void Display(Colors color = Colors.White, bool hover = true, Colors hoverColor = Colors.Lime, Colors hoverTextColor = Colors.Black)
        {

        }
    }

    class MenuItemBox : MenuItem
    {
        public Rect Box;
        public MenuItemBox(string name, Action action, Rect box) : base(name, action, box)
        {
            Name = name;
            this.action = action;
            Box = box;
        }

        /*public override bool IsClicked()
        {
            return base.IsClicked();
        }*/
        public override void Display(Colors color = Colors.White, bool hover = true, Colors hoverColor = Colors.Lime, Colors hoverTextColor = Colors.Black)
        {
            if (Box.pos == null) return;
            //if (!isInvoked)
            //{
            if (Program.PointInRect(Program.MousePosition(), Box) && hover)
            {
                Color.Pencil(hoverColor);
                Program.DrawFullRect(Box);
                Color.Pencil(hoverTextColor);
                Program.DrawText(Name, new(Box.pos.x + 10, Box.pos.y + 10));
                if (Program.MouseLeftPressed())
                {
                    //isInvoked = true;
                    if (Enum.TryParse(Name, true, out Games game))
                    {
                        IsExecuted = true;
                        Program.RunningGame = game;
                    }
                }
            }
            else
            {
                Color.Pencil(color);
                Program.DrawRect(Box);
                Program.DrawText(Name, new(Box.pos.x + 10, Box.pos.y + 10));
            }
            return;
            //}
            //action.Invoke();
        }
    }

    class MenuItemCircle : MenuItem
    {
        public bool IsChecked = false;
        public Circle CheckPoint;

        public MenuItemCircle(string name, Action action, Circle cercle) : base(name, action, cercle)
        {
            Name = name;
            this.action = action;
            CheckPoint = cercle;
        }

        public override void Display(Colors color = Colors.White, bool hover = true, Colors hoverColor = Colors.Lime, Colors hoverTextColor = Colors.Black)
        {
            if (CheckPoint.pos == null) return;
            //if (!isInvoked)
            {
                if (Program.PointInCircle(Program.MousePosition(), CheckPoint) && hover)
                {
                    Color.Pencil(hoverColor);
                    Program.DrawFullCircle(CheckPoint);
                    Color.Pencil(hoverTextColor);
                    Program.DrawText(Name, new(CheckPoint.pos.x + 10, CheckPoint.pos.y + 10));
                    if (Program.MouseLeftPressed())
                    {
                        //isInvoked = true;
                        if (Enum.TryParse(Name, true, out Games game))
                        {
                            Program.RunningGame = game;
                        }
                    }
                }
                else
                {
                    Color.Pencil(color);
                    Program.DrawCircle(CheckPoint);
                    Program.DrawText(Name, new(CheckPoint.pos.x + 10, CheckPoint.pos.y + 10));
                }
                return;
            }
            //action.Invoke();
        }
    }
}

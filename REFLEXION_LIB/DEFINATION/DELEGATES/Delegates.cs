using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public delegate void BallLocChanged(Page pg, BallLocChangedEventArgs e);

    public delegate void PaintedEvent(Game game, PaintedEventArgs e);

    public delegate void PageChangedEvent(Game game, PageChangedEventArgs e);

    public delegate void StateChanged(Object.BaseObject obj, StateChangedArgs e);

    public delegate void GameStateChanged(Game game, GameStateChangedEventArgs e);

    public delegate void Messaging(Game game, MessagingEventArgs e);
}
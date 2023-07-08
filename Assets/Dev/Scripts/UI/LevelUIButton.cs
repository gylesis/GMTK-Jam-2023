namespace Dev.Scripts.UI
{
    public class LevelUIButton : ReactiveButton<LevelUIView>
    {
        private LevelUIView _levelUIView;
        protected override LevelUIView Value => _levelUIView;

        public void Init(LevelUIView levelUIView)
        {
            _levelUIView = levelUIView;
        }
        
    }
}
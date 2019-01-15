namespace abasilak
{
    public class AnimationGUI
    {
        #region Private Properties
        bool _pause;
        bool _play;
        bool _animation_stop;
        #endregion

        #region Public Properties
        public bool play
        {
            get
            {
                return _play;
            }
            set
            {
                _play = value;
            }
        }
        public bool animation_stop
        {
            get
            {
                return _animation_stop;
            }
            set
            {
                _animation_stop = value;
            }
        }
        public bool pause
        {
            get
            {
                return _pause;
            }
            set
            {
                _pause = value;
            }
        }
        #endregion

        #region Constructor
        public AnimationGUI()
        {
            _pause = false;
            _play = false;
            _animation_stop = false;
        }
        #endregion
    }
}
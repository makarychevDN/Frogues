using UnityEngine;

namespace FroguesFramework
{
    public class DivIntContainer : IntContainer
    {
        [SerializeField] private IntContainer dividend;
        [SerializeField] private IntContainer divisor;
        private int _hashedValue;
        
        public override int Content
        {
            get
            {
                _hashedValue = dividend.Content / divisor.Content;
                Content = _hashedValue;
                return _hashedValue;
            }
        }
    }
}

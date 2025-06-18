namespace SwiftCart.Services
{

    // This service is used to manage and share state across components in the application.
    public class SharedStateService
    {
        // This method is used to notify subscribers that the state has changed.
        // No one is right now listening to this event
        public event Action OnChange;
        // Now How to hook on this event?

        private int _totalCartCount;

        public int TotalCartCount
        {
            get => _totalCartCount;
            set
            {
                _totalCartCount = value;
                NotifyStateChanged(); 
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();


    }
}

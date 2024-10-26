namespace Interfaces
{
    public interface MainUIPanel
    {   
        //Returns true if it has consumed the input
        public bool ConfirmInput(bool fromMouse);
        //Returns true if it has consumed the input
        public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse);

        public void Toggle(bool setActive);
    }
}
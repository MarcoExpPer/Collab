namespace Interfaces
{
    public interface ISelectableUI
    {   
        //Returns true if it has consumed the input
        public bool ConfirmInput(bool fromMouse);
        //Returns true if it has consumed the input
        public bool ItemInput(EEquipedItemSlot itemSlot, bool fromMouse);

        public bool IsSelectable();
    }
}
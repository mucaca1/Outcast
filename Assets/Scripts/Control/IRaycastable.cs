namespace Outcast.Control {
    public interface IRaycastable {

        CursorType GetCursorType();
        public bool HandleRaycast(PlayerController controller);
    }
}
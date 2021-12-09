namespace MVVMCore
{
    /// <summary>
    /// Akcje paska przewijania.
    /// </summary>
    public enum ScrollAction
    {
        /// <summary>
        /// Niezdefiniowany.
        /// </summary>
        None = 0,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content downward by one line.
        /// </summary>
        LineDown = 1,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the left by a predetermined amount.
        /// </summary>
        LineLeft = 2,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the right by a predetermined amount.
        /// </summary>
        LineRight = 3,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content upward by one line.
        /// </summary>
        LineUp = 4,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content downward by one page.
        /// </summary>
        PageDown = 5,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the left by one page.
        /// </summary>
        PageLeft = 6,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content to the right by one page.
        /// </summary>
        PageRight = 7,
        /// <summary>
        /// Scrolls the ScrollViewerFresh content upward by one page.
        /// </summary>
        PageUp = 8,
        /// <summary>
        /// Scrolls vertically to the end of the ScrollViewerFresh content.
        /// </summary>
        ToBottom = 9,
        /// <summary>
        /// Scrolls vertically to the end of the ScrollViewerFresh content.
        /// </summary>
        ToEnd = 10,
        /// <summary>
        /// Scrolls vertically to the beginning of the ScrollViewerFresh content.
        /// </summary>
        ToHome = 11,
        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified horizontal offset position.
        /// </summary>
        ToHorizontalOffset = 12,
        /// <summary>
        /// Scrolls horizontally to the beginning of the ScrollViewerFresh content.
        /// </summary>
        ToLeftEnd = 13,
        /// <summary>
        /// Scrolls horizontally to the end of the ScrollViewerFresh content.
        /// </summary>
        ToRightEnd = 14,
        /// <summary>
        /// Scrolls vertically to the beginning of the ScrollViewerFresh content.
        /// </summary>
        ToTop = 15,
        /// <summary>
        /// Scrolls the content within the ScrollViewerFresh to the specified vertical offset position.
        /// </summary>
        ToVerticalOffset = 16,
    }
}

namespace CARGAME.Inputs
{
    //Game Input Interface
    public interface IInputs
    {
        /// <summary>
        /// Presing left button
        /// </summary>
        void TurnLeftButtonPressed();

        /// <summary>
        /// Presing right button
        /// </summary>
        void TurnRightButtonPressed();

        /// <summary>
        /// Relasing left button
        /// </summary>
        void TurnLeftButtonRelased();

        /// <summary>
        /// Relasing right button
        /// </summary>
        void TurnRightButtonRelased();
    }   
}
using RMC.DOTS.Samples.Games.ShootEmUp2D;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Common world elements
    /// </summary>
    public class Common: MonoBehaviour
    {
        //  Events ----------------------------------------

        
        //  Properties ------------------------------------
        public MainUI MainUI { get { return _mainUI; }}

        
        //  Fields ----------------------------------------
        [SerializeField]
        private MainUI _mainUI;
        
        
        //  Unity Methods  -------------------------------
        
        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}
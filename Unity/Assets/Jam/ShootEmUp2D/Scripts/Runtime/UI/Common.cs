using RMC.DOTS.Samples.Games.ShootEmUp2D;
using RMC.DOTS.Systems.Cinemachines;
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

        public CinemachineController CinemachineController { get { return _cinemachineController; }}
        
        //  Fields ----------------------------------------
        [SerializeField]
        private MainUI _mainUI;

        [SerializeField]
        private CinemachineController _cinemachineController;
        
  
        //  Unity Methods  -------------------------------
        
        
        //  Methods ---------------------------------------
        
        
        //  Event Handlers --------------------------------
    }
}
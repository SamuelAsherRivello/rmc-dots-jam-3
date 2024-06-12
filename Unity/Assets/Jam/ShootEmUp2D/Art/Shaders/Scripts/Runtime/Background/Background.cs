using System;
using UnityEngine;

namespace RMC.DOTS.Samples.Games.ShootEmUp2D
{
    //  Namespace Properties ------------------------------


    //  Class Attributes ----------------------------------


    /// <summary>
    /// Replace with comments...
    /// </summary>
    public class Background : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Vector2 ScrollSpeed
        {
            get
            {
                return _scrollSpeed;
            }
            set
            {
                //Only if the value has changed
                if (_scrollSpeed != value)
                {
                    // Store the material reference (just once)
                    if (_material == null)
                    {
                        _material = _renderer.material;
                    }
              
                    // Store the value
                    _scrollSpeed = value;
                    
                    // Update the ShaderGraph value
                    _material.SetVector("_ScrollSpeed", _scrollSpeed);
                }
        
            }
        }

        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;

                if (_isPaused)
                {
                    ScrollSpeed = new Vector2();
                }
                else
                {
                    ScrollSpeed = _initialScrollSpeed;
                }
            }
        } 

        //  Fields ----------------------------------------
        [SerializeField] 
        private Renderer _renderer;
        
        private Material _material;
        private Vector2 _scrollSpeed;
        
        [SerializeField]
        private Vector2 _initialScrollSpeed =  new Vector2(0, 0.1f);

        private bool _isPaused = false;


        //  Unity Methods ---------------------------------
        protected void Start()
        {
            //Debug.Log($"{gameObject.name}.{GetType().Name}.Start()");

            SetScrollSpeedViaInspector();
        }

        protected void OnValidate()
        {
            SetScrollSpeedViaInspector();
        }
        
        protected void OnDestroy()
        {
            //Prevent git from thinking the value has changed
            ScrollSpeed = new Vector2();
        }

        //  Methods ---------------------------------------
        
        protected void SetScrollSpeedViaInspector()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            ScrollSpeed = _initialScrollSpeed;
        }

        //  Event Handlers --------------------------------
    }
}
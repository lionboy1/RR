using UnityEngine;
using UnityEngine.UI;
using RR.Saving;
using RR.Control;

namespace RR.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float m_currHealth = 100f;
        [SerializeField] AudioSource m_deathAudio;
        [SerializeField] AudioSource m_hurtAudio;
        [SerializeField] GameObject _bloodSplashEffect;
        [SerializeField] float lowHealththreshold;
        AIController L_aiController;
        Animator _anim;
        ActionScheduler _actionScheduler;
        bool isDead = false;
        ///<summary>
        ///Health slide code
        public Slider slider;
        public Gradient gradient;
        public Image fill;
        ///</summary>

        //Getter so other classes can query if target is dead or not
        public bool IsDead()
        {
            return isDead;
        }

        void Awake()
        {
            _anim = this.GetComponent<Animator>();
            if(_anim == null)
            {
                Debug.LogError("Animator not found");
            }
            _actionScheduler = GetComponent<ActionScheduler>();
            if(_actionScheduler == null)
            {
                Debug.LogError("actionScheduler component not found!");
            }
            L_aiController = GetComponent<AIController>();
            
            //Initialize the slider at full health
            slider.maxValue = m_currHealth;
            slider.value = m_currHealth;
            //Initialize the health fill color to the color at full health(green)
            fill.color = gradient.Evaluate(1f);

        }

        public bool LowHealth()
        {
            return m_currHealth < lowHealththreshold;
        }

        public void Damage(float damage)
        {
            float L_originalHealth = m_currHealth;
            m_currHealth = Mathf.Max(m_currHealth - damage, 0);//Reduces health but not under 0
            
            slider.value = m_currHealth;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            
            ///Handle aggravating AI from separate script when player attacks enemy
            /*if(m_currHealth < L_originalHealth)
            {
                L_aiController.Aggravate();//AI is hurt and gets angry at player
            }*/
            
            if(m_currHealth == 0)
            {
               Die();
               GameObject _bloodSplashInstance = Instantiate(_bloodSplashEffect, transform.position, Quaternion.identity); 
            }
            
        }
        void Die()
        {
            if (isDead) return;//back out of the function is isDead is already true;
                
            //Otherwise, continue
            _anim.SetTrigger("die");
            isDead = true;
            m_deathAudio.Play();

            _actionScheduler.CancelCurrentAction();
        }

        public object CaptureState()
        {
            return m_currHealth;
        }

        public void RestoreState(object state)
        {
            m_currHealth = (float)state;
            //Force any character to remain dead if they were before saving
            //Or they will spawn on load with 0 health but be alive
            if(m_currHealth == 0)
            {
                
                Die();
            }
        }

        public float GetCurrentHealth()
        {
            return m_currHealth;
        }
    }
}


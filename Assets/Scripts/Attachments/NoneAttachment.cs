using HGDFall2024.LevelElements;
using HGDFall2024.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

namespace HGDFall2024.Attachments
{
    [RequireComponent(typeof(SpriteMask))]
    public class NoneAttachment : BaseAttachment
    {
        private readonly RaycastHit2D[] hits = new RaycastHit2D[5];
        
        public override AttachmentType Attachment { get; } = AttachmentType.None;

        public Material outlineMaterial;
        public float outlineWidth = 0.05f;
        public Color outlineColor = Color.yellow;

        public float grabRadius = 10;
        public float distance = 0.1f;
        public float jointDamping = 0;
        public float jointFrequency = 1;

        public float circleShowRadius = 2;
        public float alphaLerp = 3;
        public float minAlpha;
        public float maxAlpha;

        private Vector2 maxMousePosition;
        private float mouseDistance;

        private SpringJoint2D joint;
        private float lastMass;
        private Rigidbody2D hoverObject;

        private LineRenderer lineRenderer;
        private SpriteMask mask;
        private Material grabCircleMaterial;
        private float targetCircleAlpha = 0;
        private PositionConstraint circleConstraint;
        private readonly SpriteRenderer[] renderers = new SpriteRenderer[8];

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = outlineColor;
            lineRenderer.endColor = outlineColor;
            mask = GetComponent<SpriteMask>();

            ParticleSystem grabCircle = GetComponentInChildren<ParticleSystem>();
            grabCircleMaterial = grabCircle.GetComponent<ParticleSystemRenderer>().material;
            circleConstraint = grabCircle.GetComponent<PositionConstraint>();   
            // Creating a copy in 8 directions works so
            Vector2[] directions = new Vector2[8]
            {
                Vector2.up,
                new(1, 1),
                Vector2.right,
                new(1, -1),
                Vector2.down,
                new(-1, -1),
                Vector2.left,
                new(-1, 1)
            };
            for (int i = 0; i < 8; i++)
            {
                GameObject go = new("outline" + i);
                go.transform.parent = transform;
                go.transform.localPosition = directions[i] * outlineWidth;
                
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                renderer.material = outlineMaterial;
                renderer.color = outlineColor;
                renderers[i] = renderer;
            }

            ParticleSystem.ShapeModule shape = grabCircle.shape;
            NoneAttachment grabber = PlayerManager.Instance.Attachments[AttachmentType.None] as NoneAttachment;
            shape.radius = grabber.grabRadius;

            UpdateConstraint();

            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene _, Scene __)
        {
            if (PlayerManager.Instance.Player == null || circleConstraint == null)
            {
                return;
            }

            UpdateConstraint();
        }

        private void UpdateConstraint()
        {
            circleConstraint.SetSource(
                0,
                new ConstraintSource()
                {
                    sourceTransform = PlayerManager.Instance.Player.transform,
                    weight = 1
                }
            );
        }

        protected override void Update()
        {
            base.Update();

            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;
            mouseDistance = Vector2.Distance(playerPos, MousePosition);
            maxMousePosition = playerPos + Vector2.ClampMagnitude(MousePosition - playerPos, grabRadius);

            float alpha = Mathf.MoveTowards(
                grabCircleMaterial.color.a, 
                targetCircleAlpha, 
                alphaLerp * Time.deltaTime * (maxAlpha - minAlpha)
            );
            grabCircleMaterial.color = new Color(
                grabCircleMaterial.color.r,
                grabCircleMaterial.color.g,
                grabCircleMaterial.color.b,
                alpha
            );

            if (joint != null)
            {
                SetOutline(joint.gameObject);
            }
            else if (hoverObject != null)
            {
                SetOutline(hoverObject.gameObject);
            }
            else
            {
                SetOutline(null);
            }

            if (hoverObject != null 
                && InputManager.Instance.Player.Click.WasPressedThisFrame() 
                && joint == null)
            {
                lastMass = hoverObject.mass;
                hoverObject.mass /= 20;

                joint = hoverObject.AddComponent<SpringJoint2D>();
                joint.enableCollision = true;
                joint.autoConfigureDistance = false;
                joint.autoConfigureConnectedAnchor = false;
                joint.distance = distance;
                joint.dampingRatio = jointDamping;
                joint.frequency = jointFrequency / Mathf.Pow(lastMass, 0.25f);
                hoverObject = null;

                UpdateLineRenderer();
                lineRenderer.enabled = true;
            }
            else if (InputManager.Instance.Player.Click.WasReleasedThisFrame()
                && joint != null)
            {
                joint.attachedRigidbody.mass = lastMass;
                Destroy(joint);
                joint = null;

                lineRenderer.enabled = false;
            }
            else if (joint == null)
            {
                lineRenderer.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (InputManager.Instance.Player.Click.IsPressed() && joint != null)
            {
                WhileClicked();
            }
            else
            {
                CheckHover();
            }
        }

        private void WhileClicked()
        {
            UpdateLineRenderer();

            if (mouseDistance > grabRadius - circleShowRadius)
            {
                targetCircleAlpha = maxAlpha;
            }
            else
            {
                targetCircleAlpha = minAlpha;
            }
            joint.connectedAnchor = maxMousePosition;
        }

        private void CheckHover()
        {
            targetCircleAlpha = minAlpha;
            int hitCount = Physics2D.RaycastNonAlloc(MousePosition, Vector2.zero, hits, 0);
            hoverObject = null;
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.GetComponent<Pickupable>() == null)
                {
                    continue;
                }

                if (mouseDistance >= grabRadius - circleShowRadius)
                {
                    targetCircleAlpha = maxAlpha;

                    if (mouseDistance >= grabRadius)
                    {
                        hoverObject = null;
                        break;
                    }
                }
           
                hoverObject = hits[i].collider.gameObject.GetComponent<Rigidbody2D>();
                break;
            }
        }

        private void SetOutline(GameObject go)
        {
            Sprite sprite = null;
            if (go != null)
            {
                sprite = go.GetComponent<SpriteRenderer>().sprite;

                transform.SetPositionAndRotation(go.transform.position, go.transform.rotation);
                transform.localScale = go.transform.lossyScale;
            }

            mask.sprite = sprite;
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.sprite = sprite;
            }
        }

        private void UpdateLineRenderer()
        {
            lineRenderer.SetPosition(0, maxMousePosition);
            lineRenderer.SetPosition(1, joint.transform.position);
        }
    }
}

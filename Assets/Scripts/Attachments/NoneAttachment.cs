using HGDFall2024.LevelElements;
using HGDFall2024.Managers;
using UnityEngine;

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
        public float distance = 0.1f;
        public float jointDamping = 0;
        public float jointFrequency = 1;

        private SpringJoint2D joint;
        private GameObject hoverObject;

        private LineRenderer lineRenderer;
        private SpriteMask mask;
        private readonly SpriteRenderer[] renderers = new SpriteRenderer[8];

        private void Start()
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startColor = outlineColor;
            lineRenderer.endColor = outlineColor;
            mask = GetComponent<SpriteMask>();

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
        }

        protected override void Update()
        {
            base.Update();

            if (joint != null)
            {
                SetOutline(joint.transform.gameObject);
            }
            else
            {
                SetOutline(hoverObject);
            }

            if (hoverObject != null 
                && InputManager.Instance.Player.Click.WasPressedThisFrame() 
                && joint == null)
            {
                joint = hoverObject.AddComponent<SpringJoint2D>();
                joint.enableCollision = true;
                joint.autoConfigureDistance = false;
                joint.autoConfigureConnectedAnchor = false;
                joint.distance = distance;
                joint.dampingRatio = jointDamping;
                joint.frequency = jointFrequency;
                hoverObject = null;

                UpdateLineRenderer();
                lineRenderer.enabled = true;
            }
            else if (InputManager.Instance.Player.Click.WasReleasedThisFrame()
                && joint != null)
            {
                Destroy(joint);
                joint = null;

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
            joint.connectedAnchor = MousePosition;
        }

        private void CheckHover()
        {
            int hitCount = Physics2D.RaycastNonAlloc(MousePosition, Vector2.zero, hits, 0);
            GameObject hit = null;
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.GetComponent<Pickupable>() == null)
                {
                    continue;
                }

                hit = hits[i].collider.gameObject;
                break;
            }

            hoverObject = hit;
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
            lineRenderer.SetPosition(0, MousePosition);
            lineRenderer.SetPosition(1, joint.transform.position);
        }
    }
}

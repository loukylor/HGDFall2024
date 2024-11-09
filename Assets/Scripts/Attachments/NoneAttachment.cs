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
        public float lerpSpeed = 0.3f;

        private Rigidbody2D connectedObject;
        private GameObject hoverObject;
        private LineRenderer lineRenderer;
        private SpriteMask mask;
        private readonly SpriteRenderer[] renderers = new SpriteRenderer[8];

        private void Start()
        {
            lineRenderer = GetComponentInChildren<LineRenderer>();
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

            if (connectedObject != null)
            {
                SetOutline(connectedObject.gameObject);
            }
            else
            {
                SetOutline(hoverObject);
            }

            if (hoverObject != null 
                && InputManager.Instance.Player.Click.WasPressedThisFrame() 
                && connectedObject == null)
            {
                connectedObject = hoverObject.GetComponent<Rigidbody2D>();
            }
        }

        private void FixedUpdate()
        {
            if (InputManager.Instance.Player.Click.IsPressed() && connectedObject != null)
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
            hoverObject = null;

            Vector2 diff = MousePosition - (Vector2)connectedObject.transform.position;
            connectedObject.velocity = diff * lerpSpeed / Time.fixedDeltaTime;

            lineRenderer.SetPosition(0, MousePosition);
            lineRenderer.SetPosition(1, connectedObject.transform.position);
            lineRenderer.enabled = true;
        }

        private void CheckHover()
        {
            lineRenderer.enabled = false;
            connectedObject = null;

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
    }
}

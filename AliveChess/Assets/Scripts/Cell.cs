using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class Cell : MonoBehaviour
{
    
    public class  CellData
    {
        public int PosX;
        public int PosY;
        public CellTypeEnum CellType;

        public CellData(int posX, int posY, int type)
        {
            PosX = posX;
            PosY = posY;
            if (type == 0)
                CellType = CellTypeEnum.COMMON;
            else if (type == 1)
                CellType = CellTypeEnum.GRASS;
            else if (type == 2)
                CellType = CellTypeEnum.WATER;

        }
        
        public CellData(int posX, int posY, CellTypeEnum cellType)
        {
            PosX = posX;
            PosY = posY;
            CellType = cellType;
        }

        private int CellType2Int()
        {
            if (CellType == CellTypeEnum.COMMON)
                return 0;
            if (CellType == CellTypeEnum.GRASS)
                return 1;
            return 2;
        }
        public static byte[] Serialize(object obj)
        {
            CellData c = (CellData)obj;
            byte[] result = new byte[15];
        
            BitConverter.GetBytes(c.PosX).CopyTo(result, 0);
            BitConverter.GetBytes(c.PosY).CopyTo(result, 5);
            BitConverter.GetBytes(c.CellType2Int()).CopyTo(result, 10);

            return result;
        }

        public static object Deserialize(byte[] data)
        {
            var posX = BitConverter.ToInt32(data, 0);
            var posY = BitConverter.ToInt32(data, 5);
            var type = BitConverter.ToInt32(data, 10);
            
            return new CellData(posX, posY, type);
        }
        
    }
    public enum CellTypeEnum
    {
        COMMON,
        GRASS, 
        WATER 
    }

    public Action<Vector2> ClickEvent;

    public SpriteRenderer SpriteRenderer;
    
    [Header("Images")]
    public Sprite CommonCellImage;
    public Sprite GrassCellImage;
    public Sprite WaterCellImage;

    [Header("Slow Effect")] 
    public int CommonCellSlowEffect;
    public int GrassCellSlowEffect;
    public int WaterCellSlowEffect;

    public CellTypeEnum CellType { get; set; }

    public static Cell CreateCell(GameObject prefab, CellData cellData)
    {
        var go = Instantiate(prefab, new Vector3(cellData.PosX, cellData.PosY), Quaternion.identity);
        var cell = go.GetComponent<Cell>();
        if (cell != null)
        {
            cell.CellType = cellData.CellType;
            cell.SetImage();
        }

        return cell;
    }
    private void OnEnable()
    {
        ClickEvent = vector2 => {};
    }

    private void OnMouseDown()
    {
       // if(!EventSystem.current.IsPointerOverGameObject())
        ClickEvent.Invoke(transform.position);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var king = other.GetComponent<Player>();
        if (king != null)
        {
            king.SlowEffect = GetSlowEffect();
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var king = other.collider.GetComponent<Player>();
        if (king != null)
        {
            king.SlowEffect = GetSlowEffect();
            
        }
    }

    private void SetImage()
    {
        if (CellType == CellTypeEnum.COMMON)
        {
            SpriteRenderer.sprite = CommonCellImage;
        }
        else if (CellType == CellTypeEnum.WATER)
        {
            SpriteRenderer.sprite = WaterCellImage;
        }
        else if (CellType == CellTypeEnum.GRASS)
        {
            SpriteRenderer.sprite = GrassCellImage;
        }
            
    }
    
    private int GetSlowEffect()
    {
        if (CellType == CellTypeEnum.GRASS)
            return GrassCellSlowEffect;
        if (CellType == CellTypeEnum.WATER)
            return WaterCellSlowEffect;
        return CommonCellSlowEffect;
    }
    

   
}

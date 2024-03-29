using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Tile : MonoBehaviour
{

    [SerializeField] protected SpriteRenderer Renderer;
    [SerializeField] private GameObject Highlight;
    [SerializeField] private bool IsWalkable;
    public static Tile tile;

    public BaseUnit OccupiedUnit;
    public bool Walkable => IsWalkable && OccupiedUnit == null;
    private bool isMoving;
    public virtual void Init(int x, int y)
    {

    }

    private void Start()
    {
        StartCoroutine(RegenerateMana());
        StartCoroutine(MoveRandomly());
    }



    void OnMouseEnter()
    {
        Highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        Highlight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MoveUnit(UnitManager.Instance.SelectedHero, Vector3.up));
        }

        if (Input.GetKeyDown(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MoveUnit(UnitManager.Instance.SelectedHero, Vector3.left));
        }

        if (Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MoveUnit(UnitManager.Instance.SelectedHero, Vector3.down));
        }

        if (Input.GetKeyDown(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MoveUnit(UnitManager.Instance.SelectedHero, Vector3.right));
        }
    }


    public IEnumerator MoveUnit(BaseUnit unit, Vector3 direction)
    {
        isMoving = true;
        float speed = 7f;
        float travelled = 0f;

        Vector3 startPosition = unit.transform.position;
        Vector3 endPosition = startPosition + direction;
        Tile endTile = GridManager.Instance.GetTileAtPosition(endPosition);

        if (endTile != null && endTile.Walkable)
        {
            Tile currentTile = GridManager.Instance.GetTileAtPosition(unit.transform.position);
            unit.OccupiedTile = null;
            currentTile.OccupiedUnit = null;

            // Be�ll�tjuk a flipX-et a SpriteRendereren a karakter megfelel� ir�ny�hoz
            if (direction == Vector3.left)
            {
                unit.GetComponent<SpriteRenderer>().flipX = true; // Balra ford�t�s eset�n
            }
            else if (direction == Vector3.right)
            {
                unit.GetComponent<SpriteRenderer>().flipX = false; // Jobbra ford�t�s eset�n
            }

            while (travelled < 1f)
            {
                unit.transform.position = Vector3.Lerp(startPosition, endPosition, travelled);
                travelled += Time.deltaTime * speed;
                yield return null;
            }

            unit.transform.position = endPosition;
            unit.OccupiedTile = endTile;
            endTile.OccupiedUnit = unit;

            HealtManager.Instance.UpdateHealth(10);
            HealtManager.Instance.UpdateMana(100);
            LvLManager.Instance.UpdateLvL(100);
        }

        isMoving = false;
    }


    public void SetUnit(BaseUnit unit, Vector3 direction)
    {
        if (unit.OccupiedTile != null)
        {
            unit.OccupiedTile.OccupiedUnit = null;
            unit.OccupiedTile = null;
        }


        if (direction != Vector3.zero)
        {

            StartCoroutine(MoveUnit(unit, direction));
        }
        else
        {
            unit.transform.position = transform.position;
        }

        OccupiedUnit = unit;
        unit.OccupiedTile = this;

        BaseHero CurrentHero = UnitManager.Instance.GetSelectedHero();

        if (CurrentHero == null)
        {
            if (OccupiedUnit.Faction == Faction.Hero)
            {
                UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);

            }
        }
    }


    private IEnumerator RegenerateMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            if (!isMoving && HealtManager.Instance._currentMana < HealtManager.Instance._maxMana)
            {
                HealtManager.Instance.UpdateMana(-100);
            }

            yield return new WaitForSeconds(3f);

            if (!isMoving && HealtManager.Instance._currentHealth < HealtManager.Instance._maxHealth)
            {
                HealtManager.Instance.UpdateHealth(-10);
            }
        }
    }


    private void OnMouseDown()
    {
        if (OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy)
        {
            BaseUnit playerUnit = UnitManager.Instance.SelectedHero; // A j�t�kos egys�ge
            float distance = Vector3.Distance(playerUnit.transform.position, OccupiedUnit.transform.position);
            if (distance < 1.2f) // V�ltoztathatsz az �rt�ken az egys�g �s az ellens�g k�z�tti megfelel� t�vols�g szerint
            {
                HealtManager.Instance.UpdateHealth(50);
                HealtManager.Instance.UpdateMana(500);
                LvLManager.Instance.UpdateLvL(100);

                Debug.Log("Egy enemy legy�zve");
                Destroy(OccupiedUnit.gameObject);
            }
            else
            {
                Debug.Log("Az ellens�g t�l messze van.");
            }
        }
        else
        {
            Debug.Log("xDDD");
        }

    }

    private IEnumerator MoveRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            if (!isMoving && OccupiedUnit != null && OccupiedUnit.Faction == Faction.Enemy)
            {
                EnemyMove(); // Mozgatjuk az enemyt
            }
        }
    }

    private void EnemyMove()
    {
        Vector3 randomDirection = GetRandomDirection();
        // Ha az ir�ny nem nulla, akkor mozgassuk az enemyt
        if (randomDirection != Vector3.zero)
        {
            StartCoroutine(MoveUnit(OccupiedUnit, randomDirection)); // Mozgatjuk az enemyt v�letlenszer�en
        }
    }


    private Vector3 GetRandomDirection()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right };
        List<Vector3> availableDirections = new List<Vector3>();

        // Ellen�rizz�k, hogy az ir�nyok k�z�l melyek �rv�nyesek
        foreach (Vector3 direction in directions)
        {
            Tile targetTile = GridManager.Instance.GetTileAtPosition(OccupiedUnit.transform.position + direction);
            if (targetTile != null && targetTile.Walkable)
            {
                availableDirections.Add(direction);
            }
        }

        // Ha van el�rhet� ir�ny, v�lasszunk v�letlenszer�en egyet
        if (availableDirections.Count > 0)
        {
            return availableDirections[Random.Range(0, availableDirections.Count)];
        }
        else
        {
            // Ha minden ir�ny blokkolt, maradjon a hely�n
            return Vector3.zero;
        }
    }



}


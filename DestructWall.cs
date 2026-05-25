using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class DestructWall : TileMapLayer
{
	private const float HEALTH = 4f;

	private Dictionary<Vector2I, float> _cellHP = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		// .EraseCell you might use
	}

	public void DamageToWall(Vector2I cellPos, float damage)
	{
		if(!_cellHP.ContainsKey(cellPos))
		{
			_cellHP[cellPos] = HEALTH;
			GD.Print(cellPos, " HP: ", _cellHP[cellPos]);
			return;
		}
		
		if(_cellHP[cellPos] > 0)
		{
			_cellHP[cellPos] -= damage;
			GD.Print(cellPos, " HP: ", _cellHP[cellPos]);
		}
		
		if(_cellHP[cellPos] == 0)
		{
			GD.Print("Trying: ", cellPos);
			GD.Print("Before: ", GetCellSourceId(cellPos));
			_cellHP.Remove(cellPos);
			this.SetCell(cellPos, -1);
			GD.Print("After: ", GetCellSourceId(cellPos));
			return;
		}
	}
}

using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public partial class main_scene : Node2D
{
	[Export]
	private Control Cart;

	[Export]
	private Node2D Pivot;

	[Export]
	private double ScaleX = 1;

	private double[] t;
	private double[] x;
	private double[] theta;

	bool running = false;
	double time = -1;
	int lastTimeIndex = -1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!running) {
			return;
		}
	
		if (time < 0) {
			time = t[0];
			lastTimeIndex = 0;
		} else {
			time += delta;
		}

		// Get current data points to use for interpolation
		int index = -1;
		double interpolation = 0;
		for(int i = lastTimeIndex; i < t.Length - 1; i++) {
			if (time >= t[i] && time < t[i + 1]) {
				index = i;
				interpolation = (time - t[i]) / (t[i + 1] - t[i]);
				break;
			}
		}
		if (index < 0) {
			// Just display the last frame and stop running
			index = t.Length - 2;
			interpolation = 1.0;
			running = false;
		}
		lastTimeIndex = index;

		// Calculate cart position
		double x = this.x[index] + interpolation * (this.x[index + 1] - this.x[index]);
		x *= ScaleX;
		double theta = this.theta[index] + interpolation * (this.theta[index + 1] - this.theta[index]);

		Cart.Position = new((float)x, Cart.Position.Y);
		Pivot.Rotation = (float)theta;
	}

	private void _on_button_pressed() {
		const string folder = "";//"C:/Users/Zachary Carey/Downloads/";
		/*
				string file = File.ReadAllText(folder + "matlab_x.csv");
				Func<string, IEnumerable<double>> filter = (string input) => input.Split("\r\n").Skip(1).Where(x => !string.IsNullOrEmpty(x)).SelectMany(x => x.Split(",").Select(double.Parse));

				double[] values = filter(file).ToArray();
				this.t = new double[values.Length / 2];
				this.x = new double[values.Length / 2];
				this.theta = new double[values.Length / 2];

				for (int i = 0; i < values.Length; i += 2) {
					t[i / 2] = values[i];
					x[i / 2] = values[i + 1];
				}

				file = File.ReadAllText(folder + "matlab_theta.csv");
				values = filter(file).ToArray();
				for(int i = 0; i < values.Length; i += 2) {
					double t = values[i];
					if (Math.Abs(t - this.t[i / 2]) > 0.01) {
						GD.PrintErr("Times differ.");
					}
					theta[i / 2] = values[i + 1];
				}*/

		string file = File.ReadAllText(folder + "matlab_out.csv");
		Func<string, IEnumerable<double>> filter = (string input) => input.Split("\r\n").Skip(1).Where(x => !string.IsNullOrEmpty(x)).SelectMany(x => x.Split(",").Select(double.Parse));

		double[] values = filter(file).ToArray();
		int rows = values.Length / 4;
		this.t = new double[rows];
		this.x = new double[rows];
		this.theta = new double[rows];
		double[] u = new double[rows];

		for (int i = 0; i < values.Length; i += 4) {
			t[i / 4] = values[i];
			x[i / 4] = values[i + 1];
			theta[i / 4] = values[i + 2];
			u[i / 4] = values[i + 3];
		}

		this.time = -1;
		running = true;
	}
}




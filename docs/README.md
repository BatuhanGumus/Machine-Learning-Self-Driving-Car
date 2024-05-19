[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](/LICENSE)

# Machine-Learning-Self-Driving-Car

A gamified machine learning experience where the user gets to create a custom track and watch the AI learn its way through it.

https://github.com/BatuhanGumus/Machine-Learning-Self-Driving-Car/assets/45213285/9f08c53b-abe9-4c1f-9148-d1d2b37f7293

> [!WARNING]  
> I made this game such a long time ago while self-learning machine learning to make a neural network from scratch.
> I can almost guarantee that this will not work in newer Unity versions.

## Game Features
1. Generate custom track with the track tools.
   - start and finish line.
   - reward lines.
   - helpful undo and loop features.
2. Begin training!
3. Speed up the simulation and watch as the cars get better at driving as the generations go by.

## Machine Learning Details
### Inputs
1) several raycasts from the car to get the distance to walls
2) cars normalized speed
### Outputs
1) cars acceleration
2) steering rotation
### Activation Method

<img src="https://github.com/BatuhanGumus/Machine-Learning-Self-Driving-Car/assets/45213285/b857c034-cf6d-404c-8655-14bd0f1d02f4" align="right" height="160px">
Hyperbolic tangent activation! A range from -1 to 1 felt adequate for gas and break acceleration and steering rotation.

```cs
private double Activation(double inp)
{
  return Math.Tanh(inp); //Hyperbolic tangent activation
}
```

### Scoring
Passing a green line will add points, wasting time and hitting the wall will substract points.
### Learning Method
Simple mitotis evolution! Kill the worse performing half and copy and mutate the better performing half.

## Future Plans
- A challange mode to test the AI cars that you thought.
- Smoother track building experience.
- GPU based Neural Network.
- AI saving to be able to close the game and continue teaching later.

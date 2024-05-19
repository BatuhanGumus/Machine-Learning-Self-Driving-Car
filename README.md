# Machine-Learning-Self-Driving-Car
[![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](/LICENSE)

A gamified machine learning experience where the user gets to create a custom track and watch the AI learn its way through it.

video here

> [!WARNING]  
> I made this game such a long time ago while self-learning machine learning to make a neural network from scratch
> I can almost guarantee that this will not work in newer Unity versions

## Neural network
### Inputs
1) several raycasts from the car to get the distance to walls
2) cars normalized speed
### Outputs
1) cars acceleration
2) steering rotation
### Scoring
Passing a green line will add points, wasting time and hitting the wall will remove points.
### Learning Method
Simple mitotis evolution! Kill the worse performing half and copy and mutate the better performing half.

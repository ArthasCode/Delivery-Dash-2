# Presentation
A markov process decision for the game Delivery Dash (a project that my Final Paper teammate made for studies) using PPO and ML-Agents.

# Training
- The project has three different agents: a trained with closest target observations (costumer or package; 6 space size observation), other trained without it (4 space size observation and unmarking the box to use the closest observations) and other with less sensors observations (along the best results).
- Those two firsts have: RayPerceptionSensor with 6 rays per direction and 2 stacked raycasts, RayPerceptionSensorBackLong with 4 rays per direction and 2 stacked raycasts (probably) and don't have RayPerceptionSensorBackNormal.
- The last has: RayPerceptionSensor with 3 rays per direction and 2 stacked raycasts, RayPerceptionSensorBackNormal with 3 rays per direction and 2 stacked raycasts and RayPerceptionSensorBackLong with 5 rays per direction and 2 stacked raycasts.
- For rewards, the intrinsic reward technique "curiosity" has been used along with extrinsic environment rewards.

## Diagram
- The diagram of markov process is below.
(insert markov process later here).

# Results
For this section, follows the tensorboard analysis
(insert chart here later).

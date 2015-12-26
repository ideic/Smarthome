# Smarthome
Smarthome is a project how to manage electric things in the house.

As a first step I focused on lamps and their switches.

There are many lamps and switches in a house, in some cases multiple switches control the same light or multiple lights are controlled by the same switch.

This project uses Arduino nano to query switch's state and to control relays.

A simple Windows application helps you to setup, the topology of the house with switches and lamps.

See Documents folder for Details.

Benefits over a standard alternate switch or traditional switch - light connection:

- Switch controlled by 3,3 V => safe
- switch and light connection are dynamic => you can change it without phisical act
- cheap ( arduino, utp cable (for communication and to switches, lights need small diameter cable)
- Additional feature can be intorduced, like random light on/off, to imitate owners at home (safe home), or you can be notified if someone uses the lights, or you can remote control it => possibilities are endless (but not shipped yet)
 

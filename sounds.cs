datablock AudioProfile(CA_stepSound)
{
	filename = "./sounds/beep.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(CA_popSound : CA_stepSound) { filename = "./sounds/pop.wav"; };
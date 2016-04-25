datablock AudioProfile(CA_stepSound)
{
	filename = "./sounds/beep.wav";
	description = AudioClosest3d;
	preload = true;
};

datablock AudioProfile(CA_popSound : CA_stepSound) { filename = "./sounds/pop.wav"; };
datablock AudioProfile(CA_countdownSound : CA_stepSound) { filename = "./sounds/countdown.wav"; };
datablock AudioProfile(CA_goSound : CA_stepSound) { filename = "./sounds/go.wav"; };
datablock AudioProfile(CA_clickSound : CA_stepSound) { filename = "./sounds/click.wav"; };
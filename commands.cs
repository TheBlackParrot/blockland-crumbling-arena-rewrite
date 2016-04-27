function serverCmdHelp(%client) {
	if($Sim::Time - %client.lastHelpCmd < 0.5) {
		return;
	}
	%client.lastHelpCmd = $Sim::Time;

	messageClient(%client, '', "\c2/help \c7-- \c6This command");
	messageClient(%client, '', "\c2/stats \c7-- \c6List your stats");
}

function serverCmdStats(%client) {
	if($Sim::Time - %client.lastStatsCmd < 0.5) {
		return;
	}
	%client.lastStatsCmd = $Sim::Time;

	%font = "<font:Courier New Bold:24>";
	messageClient(%client, '', %font @ "\c2Wins:          \c6" @ %client.wins);
	messageClient(%client, '', %font @ "\c2Bricks broken: \c6" @ %client.brokenBricks);
}
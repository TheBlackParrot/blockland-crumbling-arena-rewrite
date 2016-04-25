function MinigameSO::newRound(%this) {
	%this.rounds++;
	BrickGroup_888888.chainDeleteAll();

	%this.messageAll('', "\c5Starting \c3round" SPC %this.rounds @ "\c5...");

	$CrumblingArena::Active = 0;

	setGradient(getRandom(0, $CrumblingArena::Gradients.length-1));
	%this.buildBoard();
}

function MinigameSO::buildBoard(%this) {
	cancel(%this.buildSched);
	if(BrickGroup_888888.getCount() > 0) {
		%this.buildSched = %this.schedule(100, buildBoard);
	} else {
		generateCABoard();
		%this.onBoardBuilt();
	}
}

function MinigameSO::onBoardBuilt(%this) {
	for(%i=0;%i<%this.numMembers;%i++) {
		%client = %this.member[%i];

		%client.spawnPlayer();
		%player = %client.player;

		%spawnPos = vectorRand(CABoardData.spawnStart, CABoardData.spawnEnd);
		%player.setTransform(%spawnPos);
	}

	%this.startRound();
}

function MinigameSO::startRound(%this) {
	%offset = mClamp(BrickGroup_888888.getCount(), 0, 10000);
	%this.activeSched = %this.schedule(10000+%offset, setGameActive, 1);
	%this.timerSched = %this.schedule(5000+%offset, startTimer);
}

function MinigameSO::startTimer(%this) {
	%this.centerPrintAll("<font:Impact:64>\c55");
	%this.schedule(1000, centerPrintAll, "<font:Impact:64>\c54");
	%this.schedule(2000, centerPrintAll, "<font:Impact:64>\c53");
	%this.schedule(3000, centerPrintAll, "<font:Impact:64>\c52");
	%this.schedule(4000, centerPrintAll, "<font:Impact:64>\c51");
	%this.schedule(5000, centerPrintAll, "<font:Impact:64>\c2START!", 1);
}

function MinigameSO::setGameActive(%this, %val) {
	$CrumblingArena::Active = mClamp(%val, 0, 1);
}

package CrumblingArenaSystem {
	function MinigameSO::reset(%this) {
		for(%i=0;%i<%this.numMembers;%i++) {
			%client = %this.member[%i];
			%player = %client.player;

			if(isObject(%player)) {
				%player.delete();
			}
		}

		%this.newRound();
	}

	function MinigameSO::checkLastManStanding(%this) {
		if(isEventPending(%this.resetSched)) {
			return;
		}

		for(%i=0;%i<%this.numMembers;%i++) {
			%client = %this.member[%i];
			
			if(isObject(%client.player)) {
				%count++;
				%last = %client;
			}
		}

		if(%count == 1) {
			%this.messageAll('', "\c3" @ %last.getName() SPC "\c5has won this round! Resetting in \c35 seconds.");
			%this.resetSched = %this.schedule(5000, reset);
			return;
		}

		if(%count > 0) {
			return;
		}

		%this.messageAll('', "\c5No one won this round. Resetting in \c35 seconds...");
		%this.resetSched = %this.schedule(5000, reset);
	}
};
activatePackage(CrumblingArenaSystem);
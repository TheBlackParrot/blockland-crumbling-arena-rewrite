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

		%client.saveCAData();
	}

	%this.amountPlayed = %this.numMembers;
	%this.startRound();
}

function MinigameSO::startRound(%this) {
	%offset = mClamp(BrickGroup_888888.getCount(), 0, 10000);
	%this.activeSched = %this.schedule(10000+%offset, setGameActive, 1);
	%this.timerSched = %this.schedule(5000+%offset, startTimer);

	$CrumblingArena::EnableSpleef = !getRandom(0, 8);
	if($CrumblingArena::EnableSpleef) {
		%this.messageAll('', "\c5This round is a \c3Spleef round\c5! Click bricks under you to make them fall!");
	}

	$CrumblingArena::EnableRockets = !getRandom(0, 8);
	if($CrumblingArena::EnableRockets) {
		%this.messageAll('', "\c5Rockets will fall from above! Watch out!");
		%this.schedule(10001+%offset, doRockets);
	}
}

function MinigameSO::doCountDown(%this, %delay, %data) {
	%this.schedule(%delay, centerPrintAll, %data);
	%this.schedule(%delay, playSound, CA_countdownSound);
}

function MinigameSO::startTimer(%this) {
	for(%i=0;%i<5;%i++) {
		%this.doCountDown(%i*1000, "<font:Impact:64>\c5" @ 5-%i);
	}
	%this.schedule(5000, playSound, CA_goSound);
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

		$CrumblingArena::WinnerDetermined = 0;
		%this.newRound();
	}

	function MinigameSO::checkLastManStanding(%this) {
		if(isEventPending(%this.resetSched) || $CrumblingArena::WinnerDetermined) {
			return;
		}

		%count = 0;
		for(%i=0;%i<%this.numMembers;%i++) {
			%client = %this.member[%i];
			
			if(isObject(%client.player)) {
				%last[%count] = %client;
				%count++;
			}
		}

		if(%count == 2) {
			%this.messageAll('', "\c5It's down to 2 players! Swords have been given out.");
			
			%last[0].player.setItem(SwordItem);
			%last[1].player.setItem(SwordItem);
			
			return;
		}

		if(%count == 1) {
			if(%this.amountPlayed > 2) {
				%last[0].wins++;
				%last[0].score = %last[0].wins;

				%add = " \c3" @ %last[0].getName() SPC "\c5has won\c3" SPC %last[0].wins SPC "time(s)!";
			}

			%this.messageAll('', "\c3" @ %last[0].getName() SPC "\c5has won this round!" @ %add @ " Resetting in \c35 seconds.");
			%this.resetSched = %this.schedule(5000, reset);

			%last[0].saveCAData();

			$CrumblingArena::WinnerDetermined = 1;
			CARockets.explode();
			return;
		}

		if(%count > 0) {
			return;
		}

		%this.messageAll('', "\c5No one won this round. Resetting in \c35 seconds...");
		%this.resetSched = %this.schedule(5000, reset);
		CARockets.explode();
	}
};
activatePackage(CrumblingArenaSystem);
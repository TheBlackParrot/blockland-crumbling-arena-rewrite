function fxDTSBrick::breakBrick(%this) {
	if(isEventPending(%this.breakSched) || !$CrumblingArena::Active) {
		return;
	}

	// eventually get around to adjusting this to a lower value as more players die
	// clamp to (50, 500)
	%this.breakSched = %this.schedule(500, fakeRemove);

	%this.setColorFX(3);
	%this.playSound(CA_stepSound);
}

function fxDTSBrick::fakeRemove(%this) {
	%this.fakeKillBrick("0 0 0");
	%this.schedule(500, delete);

	%this.playSound(CA_popSound);
}

package CrumblingArenaInteraction {
	function fxDTSBrick::onPlayerTouch(%this, %player) {
		%this.breakBrick();

		return parent::onPlayerTouch(%this, %player);
	}

	function Armor::onEnterLiquid(%this, %obj, %coverage, %type) {
		if(!$CrumblingArena::Active) {
			%spawnPos = vectorRand(CABoardData.spawnStart, CABoardData.spawnEnd);
			%obj.setTransform(%spawnPos);
		}

		if(!$CrumblingArena::Debug) {
			%obj.kill();
		}

		return parent::onEnterLiquid(%this, %obj, %coverage, %type);
	}
};
activatePackage(CrumblingArenaInteraction);
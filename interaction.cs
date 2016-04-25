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

function Player::getBrickLookedAt(%this) {
	%eye = vectorScale(%this.getEyeVector(), 100);
	%pos = %this.getEyePoint();
	%mask = $TypeMasks::FXBrickObjectType;
	%hit = getWord(containerRaycast(%pos, vectorAdd(%pos, %eye), %mask, %this), 0);

	if(isObject(%hit)) {
		return %hit;
	}

	return -1;
}

package CrumblingArenaInteraction {
	function fxDTSBrick::onPlayerTouch(%this, %player) {
		%this.breakBrick();

		return parent::onPlayerTouch(%this, %player);
	}

	function Armor::onTrigger(%this, %player, %slot, %val) {
		if($CrumblingArena::EnableSpleef) {
			if(%slot == 0 && %val) {
				%looking = %player.getBrickLookedAt();
				if(isObject(%looking)) {
					if(!isEventPending(%looking.breakSched) &&  $CrumblingArena::Active) {
						%looking.playSound(CA_clickSound);
					}
					%looking.breakBrick();
				}
			}
		}
		
		return parent::onTrigger(%this, %player, %slot, %val);
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
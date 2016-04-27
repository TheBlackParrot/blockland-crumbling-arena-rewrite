function fxDTSBrick::breakBrick(%this, %client) {
	if(isEventPending(%this.breakSched) || !$CrumblingArena::Active) {
		return;
	}

	// eventually get around to adjusting this to a lower value as more players die
	// clamp to (50, 500)
	%this.breakSched = %this.schedule($DefaultMinigame.getDeleteOffset(), fakeRemove);

	if(!%this.colorFxID) {
		%this.setColorFX(3);
	}
	if(%this.colorFxID == 4) {
		%this.setEmitter(BurnEmitterA);
	}
	%this.playSound(CA_stepSound);

	if(isObject(%client)) {
		%client.brokenBricks++;
	}
}

function fxDTSBrick::fakeRemove(%this) {
	%this.fakeKillBrick("0 0 0");
	%this.schedule(500, delete);

	if(%this.colorFxID == 4) {
		doBrickExplosion(%this.getPosition(), 3);
		serverPlay3D(rocketExplodeSound, %this.getPosition());
	}

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
		%this.breakBrick(%player.client);

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

	// Script_ClickPush
	function Player::activateStuff(%player) {
		%v = Parent::activateStuff(%player);
		%client = %player.client;

		if($Sim::Time - %player.lastClick < 0.3) {
			return %v;
		}
		%player.lastClick = $Sim::Time;
		
		if(!$CrumblingArena::Active) {
			return %v;
		}
		
		%target = containerRayCast(%player.getEyePoint(),vectorAdd(vectorScale(vectorNormalize(%player.getEyeVector()),2),%player.getEyePoint()),$TypeMasks::PlayerObjectType,%player);
		
		if(!isObject(%target) || %target == %player || %player.getObjectMount() == %target) {
			return %v;
		}
		
		%target.setVelocity(vectorAdd(%target.getVelocity(),vectorScale(%player.getEyeVector(),7)));
		
		return %v;
	}

	function Armor::onEnterLiquid(%this, %obj, %coverage, %type) {
		if(!$CrumblingArena::Active) {
			%spawnPos = vectorRand(CABoardData.spawnStart, CABoardData.spawnEnd);
			%obj.setTransform(%spawnPos);

			return parent::onEnterLiquid(%this, %obj, %coverage, %type);
		}

		if(!$CrumblingArena::Debug) {
			%obj.kill();
		}

		return parent::onEnterLiquid(%this, %obj, %coverage, %type);
	}
};
activatePackage(CrumblingArenaInteraction);
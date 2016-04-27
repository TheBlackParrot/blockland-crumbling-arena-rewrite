if(isObject(gravityRocketProjectile)) {
	gravityRocketExplosion.damageRadius = 0;
	gravityRocketExplosion.radiusDamage = 0;
	gravityRocketExplosion.impulseRadius = 3;
	gravityRocketExplosion.impulseForce = 1500;
	gravityRocketExplosion.playerBurnTime = 1000;

	gravityRocketProjectile.directDamage = 0;
}

if(!isObject(CARockets)) {
	new ScriptGroup(CARockets);
}

function CARockets::explode(%this) {
	while(%this.getCount() > 0) {
		%row = %this.getObject(0);

		if(isObject(%row.projectile)) {
			%row.projectile.explode();
		}
		%row.delete();
	}
}

function spawnRocket(%pos) {
	%rocket = new Projectile() {
		dataBlock = gravityRocketProjectile;
		initialPosition = %pos;
		initialVelocity = "0 0 5";
		sourceObject = "";
		client = "";
		sourceSlot = 0;
		originPoint = %pos;
	};

	MissionCleanup.add(%rocket);

	%row = new ScriptObject(CARocket) {
		projectile = %rocket;
	};

	%rocket.identifier = %row;
	CARockets.add(%row);
}

function MinigameSO::doRockets(%this) {
	cancel(%this.rocketSched);

	if(!$CrumblingArena::Active) {
		return;
	}

	%this.rocketSched = %this.schedule(5000 + getRandom(0, 5000), doRockets);

	%pos = vectorAdd(vectorRand(CABoardData.spawnStart, CABoardData.spawnEnd), "0 0 100");
	spawnRocket(%pos);
}

function doBrickExplosion(%pos, %radius) {
	if(!%radius) {
		%radius = 4;
	}

	%mask = $TypeMasks::FxBrickObjectType;
	initContainerRadiusSearch(vectorSub(%pos, "0 0 0.5"), %radius, %mask);
	while(%hit = containerSearchNext()) {
		if(isObject(%hit)) {
			%hit.breakBrick();
		}
	}
}

package CrumblingArenaRocketPackage {
	function gravityRocketProjectile::onExplode(%data, %proj, %pos)
	{
		doBrickExplosion(%pos);

		if(isObject(%proj.identifier)) {
			%proj.identifier.delete();
		}

		parent::onExplode(%data, %proj, %pos);
	}
};
activatePackage(CrumblingArenaRocketPackage);
if($Pref::Server::CrumblingArena::AllowedBricks $= "") {
	$Pref::Server::CrumblingArena::AllowedBricks = "2x2 2x4 2x6 2x8 4x4 4x8 8x8";
}

exec("./sounds.cs");
exec("./gradients.cs");
exec("./board.cs");
exec("./system.cs");
exec("./interaction.cs");

function vectorRand(%v0, %v1) {
	%rx = getRandom(getWord(%v0, 0), getWord(%v1, 0));
	%ry = getRandom(getWord(%v0, 1), getWord(%v1, 1));
	%rz = getRandom(getWord(%v0, 2), getWord(%v1, 2));

	return %rx SPC %ry SPC %rz;
}

package CrumblingArenaServerPackage {
	function onServerDestroyed() {
		deleteVariables("$CrumblingArena::*");
		return parent::onServerDestroyed();
	}
};
activatePackage(CrumblingArenaServerPackage);
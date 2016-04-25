if($Pref::Server::CrumblingArena::AllowedBricks $= "") {
	$Pref::Server::CrumblingArena::AllowedBricks = "2x2 2x4 2x6 2x8 4x4 4x8 8x8";
}

exec("./support.cs");
exec("./sounds.cs");
exec("./gradients.cs");
exec("./board.cs");
exec("./system.cs");
exec("./interaction.cs");
exec("./saving.cs");
exec("./rockets.cs");

package CrumblingArenaServerPackage {
	function onServerDestroyed() {
		deleteVariables("$CrumblingArena::*");
		return parent::onServerDestroyed();
	}
};
activatePackage(CrumblingArenaServerPackage);
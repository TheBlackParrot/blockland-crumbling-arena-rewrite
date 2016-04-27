function GameConnection::loadCAData(%this) {
	%filename = "config/server/CrumblingArena/saves/" @ %this.bl_id;

	if(!isFile(%filename)) {
		return;
	}

	%file = new FileObject();
	%file.openForRead(%filename);

	%this.wins = %file.readLine();
	%this.score = %this.wins;
	messageClient(%this, '', "\c6Your save from\c3" SPC %file.readLine() SPC "\c6has been loaded.");
	%this.brokenBricks = %file.readLine();

	%file.close();
	%file.delete();
}

function GameConnection::saveCAData(%this) {
	%filename = "config/server/CrumblingArena/saves/" @ %this.bl_id;

	%file = new FileObject();
	%file.openForWrite(%filename);

	%file.writeLine(%this.wins);
	%file.writeLine(getDateTime());
	%file.writeLine(%this.brokenBricks);

	%file.close();
	%file.delete();
}

package CrumblingArenaSavingPackage {
	function GameConnection::autoAdminCheck(%this) {
		%this.loadCAData();

		return parent::autoAdminCheck(%this);
	}

	function GameConnection::onClientLeaveGame(%this) {
		%this.saveCAData();
		
		return parent::onClientLeaveGame(%this);
	}
};
activatePackage(CrumblingArenaSavingPackage);
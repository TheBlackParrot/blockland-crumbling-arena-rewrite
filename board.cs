if(!isObject(CABoardData)) {
	new ScriptObject(CABoardData);
}

function generateCABoard() {
	%mg = $DefaultMinigame;
	%allowed = $Pref::Server::CrumblingArena::AllowedBricks;

	%width = CABoardData.width = 12 + getRandom(mFloor(%mg.numMembers / 3), mFloor((%mg.numMembers / 3)*1.5));
	%length = CABoardData.length = 12 + getRandom(mFloor(%mg.numMembers / 3), mFloor((%mg.numMembers / 3)*1.5));
	%height = CABoardData.height = getRandom((%mg.numMembers > 4 ? 2 : 1), 16);

	%brickdata = "brick" @ getWord(%allowed, getRandom(0, getWordCount(%allowed)-1));
	if(!getRandom(0, 2)) {
		%plate = "f";
	}
	%brickdata = (isObject(%brickdata @ "fData") ? %brickdata @ %plate @ "Data" : %brickdata @ "Data");

	%inverse = getRandom(0, 1);
	%spaced = !getRandom(0, 8);

	if($CrumblingArena::Debug) {
		talk("GENERATING:" SPC %width @ "x" @ %length @ "x" @ %height SPC %brickdata SPC (%inverse ? "INVERSE" : "") SPC (%spaced ? "SPACED" : ""));
	}

	%sp_z = ((%height-1)*(%brickdata.BrickSizeZ/5) + 12);
	if(%spaced) {
		%sp_z = ((%height-1)*15*(%brickdata.BrickSizeZ/5) + 12);
	}
	CABoardData.spawnStart = %brickdata.BrickSizeX/2 SPC %brickdata.BrickSizeY/2 SPC %sp_z;
	CABoardData.spawnEnd = ((%width-1)*%brickdata.BrickSizeX/2) SPC ((%length-1)*%brickdata.BrickSizeY/2) SPC %sp_z;

	for(%x=0;%x<%width;%x++) {
		for(%y=0;%y<%length;%y++) {
			for(%z=0;%z<%height;%z++) {
				if(%spaced) {
					%pos = %x*(%brickdata.BrickSizeX/2) SPC %y*(%brickdata.BrickSizeY/2) SPC (%z*15*(%brickdata.BrickSizeZ/5) + 10);
				} else {
					%pos = %x*(%brickdata.BrickSizeX/2) SPC %y*(%brickdata.BrickSizeY/2) SPC (%z*(%brickdata.BrickSizeZ/5) + 10);
				}

				%brick = new fxDTSBrick(CrumblingArenaBrick) {
					angleID = 0;
					colorFxID = 0;
					colorID = getGradientPart(%z, %height, %inverse);
					dataBlock = %brickdata;
					enableTouch = 1;
					isBasePlate = 0;
					isPlanted = 1;
					position = %pos;
					printID = 0;
					scale = "1 1 1";
					shapeFxID = 0;
					stackBL_ID = 888888;
				};

				BrickGroup_888888.add(%brick);

				%brick.setTrusted(1);
				%brick.plant();
			}
		}
	}
}
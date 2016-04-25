function vectorRand(%v0, %v1) {
	%rx = getRandom(getWord(%v0, 0), getWord(%v1, 0));
	%ry = getRandom(getWord(%v0, 1), getWord(%v1, 1));
	%rz = getRandom(getWord(%v0, 2), getWord(%v1, 2));

	return %rx SPC %ry SPC %rz;
}

function MinigameSO::playSound(%this, %data) {
	if(!isObject(%data)) {
		return;
	}

	for(%i=0;%i<%this.numMembers;%i++) {
		%this.member[%i].play2D(%data);
	}
}

// Event_SetItem
function Player::setItem(%this,%item) {
	if(isObject(%this)) {
		if(%item==-1) {
			%this.updateArm(0);
			%this.unMountImage(0);
		}
		else {
			%this.updateArm(%item.image);
			%this.mountImage(%item.image,0);
		}
	}
}
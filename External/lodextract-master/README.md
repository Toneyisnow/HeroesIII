This is a set of scripts which shows how to unpack all bitmaps and animations
of Heroes of Might and Magic 3 into PNG images and then back into the formats
understood by VCMI.

These scripts are probably the first open source implementation of a writer for
the Heroes of Might and Magic 3 animation format called DEF. They are meant to
make it possible for artists to create a free replacement for the proprietary
assets VCMI currently needs.

Install VCMI and then install original game files via any of the following
methods:

	vcmibuilder --cd1 /path/to/iso/or/cd --cd2 /path/to/second/cd
	vcmibuilder --gog /path/to/gog.com/installer
	vcmibuilder --data /path/to/h3/data

This will extract the lod archives to `~/.local/share/vcmi/Data`.

Symlink sprites to Data directory so that we don't have to distribute the files
between the two directories.

	ln -s Data ~/.local/share/vcmi/Sprites

Move original archives:

	mkdir ~/lods
	mv ~/.local/share/vcmi/Data/*.lod ~/lods

Extract archives:

	for f in ~/lods/*.lod; do python lodextract.py "$f" ~/.local/share/vcmi/Data; done

(optional) Test if everything still works. At this point you should be able to
play the game and see the original graphics even though the original `*.lod`
files are elsewhere.

Backup original DEFs:

	mkdir ~/defs
	mv ~/.local/share/vcmi/Data/*.def ~/defs

Remove two broken and unused def files (they have a higher offsets than size):

	rm ~/defs/sgtwmta.def ~/defs/sgtwmtb.def

Extract all DEFs into JSON files and directories with PNG images:

	for f in ~/defs/*; do python defextract.py "$f" ~/.local/share/vcmi/Data/ || break; done

(optional) modify all frames:

	for d in ~/.local/share/vcmi/Data/*.dir; do python shred.py $d || break; done

(optional) modify all bitmaps:

	for f in ~/.local/share/vcmi/Data/*.png; do python shred.py $f || break; done

Repack all JSON:

	for f in ~/.local/share/vcmi/Data/*.json; do python makedef.py $f ~/.local/share/vcmi/Data/ || break; done

In case you followed the optional steps, enjoy your LSD infused game now :)

After above steps you will have a mixture of DEF files as well as JSON
files and their *.dir data directories. All parts of vcmi that support it will
read the animations from the JSON files now. All others will fall back to
reading the DEF files.

You can now make changes to either the PNG images in the Data directory or in
the *.dir subdirectories. If you make changes to PNG images in *.dir
subdirectories you might have to repack them into DEF files for all animations
which do not support JSON animations yet.

I only tested these scripts on Linux because I do not own a license for Windows
or MacOS. Patches welcome.

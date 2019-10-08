#!/usr/bin/env python
#
# Copyright (C) 2014  Johannes Schauer <j.schauer@email.de>
#
# This program is free software; you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation; either version 2 of the License, or
# (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License along
# with this program; if not, write to the Free Software Foundation, Inc.,
# 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

import zlib
import struct
import os
from PIL import Image, ImageDraw

def is_pcx(data):
    size,width,height = struct.unpack("<III",data[:12])
    return size == width*height or size == width*height*3

def read_pcx(data):
    size,width,height = struct.unpack("<III",data[:12])
    if size == width*height:
        im = Image.fromstring('P', (width,height),data[12:12+width*height])
        palette = []
        for i in range(256):
            offset=12+width*height+i*3
            r,g,b = struct.unpack("<BBB",data[offset:offset+3])
            palette.extend((r,g,b))
        im.putpalette(palette)
        return im
    elif size == width*height*3:
        return Image.fromstring('RGB', (width,height),data[12:])
    else:
        return None

def decompress(infile):
    fpt = open(infile)
    
    
    fpt.seek(0, 2) # move the cursor to the end of the file
    csize = fpt.tell()
    print "total size:", csize
    
    fpt.seek(0)
    rawData = fpt.read(csize)
    data = zlib.decompress(rawData)
    
    with open(infile,"w+") as o:
        o.write(data)
        
    
    return True

if __name__ == '__main__':
    import sys
    if len(sys.argv) != 2:
        print "usage: %s infile.lod ./outdir"%sys.argv[0]
        print ""
        print "usually after installing the normal way:"
        print "    %s .vcmi/Data/H3bitmap.lod .vcmi/Mods/vcmi/Data/"%sys.argv[0]
        print "    rm .vcmi/Data/H3bitmap.lod"
        print "    %s .vcmi/Data/H3sprite.lod .vcmi/Mods/vcmi/Data/"%sys.argv[0]
        print "    rm .vcmi/Data/H3sprite.lod"
        exit(1)
    ret = decompress(sys.argv[1])
    exit(0 if ret else 1)

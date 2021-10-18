#!/bin/bash
set -e

cd "$(dirname "$0")"

echo "Creating app for version $DREAMNEXUS_VERSION"

# Create the app folder
mkdir -p DreamNexus.app/Contents/MacOS
mkdir -p DreamNexus.app/Contents/Resources

# Copy contents
cp -r ../bin/Release/net5.0/osx-x64/publish/* DreamNexus.app/Contents/MacOS

# Stupid library search path workaround with a shell script
cat > DreamNexus.app/Contents/MacOS/run_dreamnexus << EOF
#!/bin/sh
export DYLD_LIBRARY_PATH="\$DYLD_LIBRARY_PATH:\$(dirname \$0)"
export GTK_DATA_PREFIX="\$(dirname \$0)" # Required to resolve themes for some reason
export XDG_DATA_DIRS="\$(dirname \$0)/share"

cd "\$(dirname \$0)"
# Run the DreamNexus binary
"\$(dirname \$0)/DreamNexus"
EOF

chmod +x DreamNexus.app/Contents/MacOS/run_dreamnexus

# Write Info.plist
cat > DreamNexus.app/Contents/Info.plist << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
	<key>CFBundleDisplayName</key>
	<string>DreamNexus</string>
	<key>CFBundleExecutable</key>
	<string>run_dreamnexus</string>
	<key>CFBundleIconFile</key>
	<string>dreamnexus.icns</string>
	<key>CFBundleIdentifier</key>
	<string>app.dreamnexus.dreamnexus</string>
	<key>CFBundleInfoDictionaryVersion</key>
	<string>6.0</string>
	<key>CFBundleName</key>
	<string>DreamNexus</string>
	<key>CFBundlePackageType</key>
	<string>APPL</string>
	<key>CFBundleShortVersionString</key>
	<string>${DREAMNEXUS_VERSION}</string>
	<key>NSHighResolutionCapable</key>
	<true/>
</dict>
</plist>

EOF

# Copy GTK libraries
cat ./mac-deps.txt | while read line 
do
  cp $line DreamNexus.app/Contents/MacOS/
done

mkdir -p DreamNexus.app/Contents/MacOS/share
mkdir -p DreamNexus.app/Contents/MacOS/lib/gdk-pixbuf/loaders
cp -r /usr/local/Cellar/gtk+3/*/share/* DreamNexus.app/Contents/MacOS/share/
cp -r /usr/local/Cellar/gdk-pixbuf/*/lib/gdk-pixbuf-2.0/2.10.0/loaders/* DreamNexus.app/Contents/MacOS/lib/gdk-pixbuf/loaders/
cp -r /usr/local/Cellar/gtk+3/*/etc/* DreamNexus.app/Contents/MacOS/etc/
cp -r /usr/local/Cellar/gtksourceview4/*/share/* DreamNexus.app/Contents/MacOS/share/
cp -r /usr/local/Cellar/adwaita-icon-theme/*/share/* DreamNexus.app/Contents/MacOS/share/
cp -r /usr/local/Cellar/hicolor-icon-theme/*/share/* DreamNexus.app/Contents/MacOS/share/

# I have no idea what this file is and why we need it but all file dialogs crash if we don't add it ¯\_(ツ)_/¯
cp -r gschemas.compiled DreamNexus.app/Contents/MacOS/share/glib-2.0/schemas/

# Create the icon
# https://www.codingforentrepreneurs.com/blog/create-icns-icons-for-macos-apps
mkdir dreamnexus.iconset
icons_path=../Assets/Icons/hicolor
cp $icons_path/16x16/apps/dreamnexus.png dreamnexus.iconset/icon_16x16.png
cp $icons_path/32x32/apps/dreamnexus.png dreamnexus.iconset/icon_16x16@2x.png
cp $icons_path/32x32/apps/dreamnexus.png dreamnexus.iconset/icon_32x32.png
cp $icons_path/64x64/apps/dreamnexus.png dreamnexus.iconset/icon_32x32@2x.png
cp $icons_path/128x128/apps/dreamnexus.png dreamnexus.iconset/icon_128x128.png
cp $icons_path/256x256/apps/dreamnexus.png dreamnexus.iconset/icon_128x128@2x.png
cp $icons_path/256x256/apps/dreamnexus.png dreamnexus.iconset/icon_256x256.png
cp $icons_path/512x512/apps/dreamnexus.png dreamnexus.iconset/icon_256x256@2x.png
cp $icons_path/512x512/apps/dreamnexus.png dreamnexus.iconset/icon_512x512.png

iconutil -c icns dreamnexus.iconset
rm -rf dreamnexus.iconset

cp dreamnexus.icns DreamNexus.app/Contents/Resources/
rm dreamnexus.icns

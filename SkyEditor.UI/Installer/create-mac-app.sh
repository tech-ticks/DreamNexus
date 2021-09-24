#!/bin/bash
cd "$(dirname "$0")"

# Create the app folder
mkdir -p DreamNexus.app/Contents/MacOS
mkdir -p DreamNexus.app/Contents/Resources

# Copy contents
cp -r ../bin/Release/net5.0/osx-x64/publish/* DreamNexus.app/Contents/MacOS

# Write Info.plist
cat > DreamNexus.app/Contents/Info.plist << EOF
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
	<key>CFBundleDisplayName</key>
	<string>DreamNexus</string>
	<key>CFBundleExecutable</key>
	<string>DreamNexus</string>
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

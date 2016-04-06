# Initial Page Size

Caner came up with this list to describe how initial image is placed:

| Input >> | Load Images | Scanner | PDF (Native) | PDF (Single Image) | PDF (Rendered) |
| -------- | -------- | -------- | -------- | -------- | -------- |
| Image | | | | | | 
| - Bitmap | Input | Input | N/A | Input | Rendering |
| - Pixel Size | Bitmap | Bitmap | N/A | Bitmap | Bitmap |
| - Dimensions | Calculated (fit) | Calculated (from resolution) | N/A | Calculated (fit) | Input |
| - Resolution | Unknown | Input | N/A | Unknown | From Settings (Render DPI) |
| Page | | | | | | 
| - Bitmap | Generated | Image Bitmap | Rendered | Generated | Image Bitmap |
| - Pixel Size | Calculated (fit) | Image Pixel Size | Bitmap | Calculated (fit) | Image Pixel Size |
| - Dimensions | From Settings (Default Page Dimensions) | Image Dimensions | Input | Input | Image Dimensions |
| - Resolution | Calculated (fit) | Image Resolution | Settings (Preview Render DPI) | Calculated (fit) | Image Resolution  |

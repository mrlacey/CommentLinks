# CommentLinks

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
![Works with Visual Studio 2019](https://img.shields.io/static/v1.svg?label=VS&message=2019&color=5F2E96)

Create links between any files. Useful if your project or solution contains code in multiple languages or you wish to link to documentation files also in the solution.

- Open the file
- Open the file and go to a specific line
- Open the file and go to specific text

When a comment contains the text `link:` followed by a file name, a green button will be added that when clicked will open that file.

![Partial screenshot showing the added button](./assets/button-example.png)

Aditionally:

- You can open the file at a specific line by putting `#L` and the line number (e.g. `#L25`) immediately after the file name.

```cs
// link:mapManager.js#L25
```

- You can open the file at a place where specific text is found by placing `:` immediately after the file name and then the text to search for.  
For compatibility with [text fragment anchors](https://github.com/WICG/ScrollToTextFragment) you can also use `#:~:text=` after the file name to specify text.

```cs
// link:mapManager.js:UpdateLocalData
// link:mapManager.js#:~:text=UpdateLocalData
```

- Files will be found anywhere in the solution, even other projects. If you have more than one file with the same name, you can specify the directory name the file is in too.

```cs
// link:include/mapManager.js
```

- Any words after the file name or search term should be automatically ignored. If you find that something isn't being detected correctly you can escape the name (and, optionally, search terms) by enclosing them in quotes.

```cs
// Go to link:"include/mapManager.js" and see ...
```
---

Please [raise an issue](https://github.com/mrlacey/CommentLinks/issues/new) if you have feature requests.

<p>
<strong>This is a project including two part:<br>
</strong><br>
<strong>A customize Board Editor</strong> 
</p>
<p>
User can define the default data in each cell in the board and export board data into "<em>boardData.asset</em>". And the game will use this board data to generate the first board.<br>

</p>
<p>
Each cell contains "Data" and "Move Direction".
</p>
<p>
"Data": Indicate what kind of element(circle icon) spawned on such cell when board is  created. (0:None, 1: red, 2:green, 3: blue)
</p>
<p>
"Move Direction": Indicate the direction that the element on such cell can move next.
</p>
<p>
*boardData.asset is ScriptableObject. it is a kind of data which Unity can access it easily.
</p>
<p>
<br>
<strong>Gameplay</strong>
</p>
<p>
Game logic is as following steps:
</p><ul>

<li>Create the first board with "boardData.asset".
<li>Click mouse left button to select the certain element and clean all connected same color elements at the same time.
<li>Move all exists elements follow the move direction on each cells and fill the valid empty cell with new elements. There are Iterator and Recursive algorithm can be chosen in MatrixGameManager. Both algorithm are for finding same color neighbors and delete them.
<li>Perform all moving animation of elements.</li></ul>

<p>
<a href="https://youtu.be/V8UHrdGWS_M">Demo</a>
</p>
[![](http://img.youtube.com/vi/V8UHrdGWS_M/0.jpg)](http://www.youtube.com/watch?v=V8UHrdGWS_M "")



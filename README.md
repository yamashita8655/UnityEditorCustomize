# UnityEditorCustomize
Unityの自作エディター拡張

【ObjectReplaceWindow.cs】  
〇基本機能  
・メニュー⇒ShortCutCommand/OpenObjectReplaceWindowか、Alt+2でウィンドウが開く  
・ヒエラルキ上の選択しているオブジェクト名を一括置換する  

〇オプショナル  
・上部は、階層関係なく選択したオブジェクトの作成した順で、左側に入れた数値からの連番で置換する  
・下部は、同階層内で選択したオブジェクトを、Sibling順（＝上から順番）で、左側に入れた数値からの連番で置換する  


【ShortCutCommand.cs】  
【VimModeInfoWindow.cs】  
〇基本機能  
・開いたCSファイルと、Unityシーンファイルを履歴で覚えて置き、ウィンドウから開ける  
・Projectウィンドウ上で、何かファイルを選択している状態で、alt+[を押すと、履歴を覚えて開くという動作をします  
・alt+oで、履歴一覧ウィンドウを開閉でき、ボタンを押すと、シーンファイルは今のシーンをセーブして開く、CSファイルなら左側はMONO、右側ならVIMで開くを実行します。  
ただし、MONOの方は、最初にUnityから開かないと、リンクがされない可能性が高い  


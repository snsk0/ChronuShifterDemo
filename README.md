### ChronuShifter(Only Source Code)
## ゲーム概要
3Dパズルゲームです。プレイヤーは過去と現在を切り替えることでギミックを解き、ステージをクリアしていくゲームです。
ゲームクリエイターズ甲子園様提出作品なため、詳細は以下リンクよりお確かめください。
> https://gameparade.creators-guild.com/works/1139</br>
> ※GCG甲子園提出先リンク
> 
※ゲームクリエイターズ甲子園様
> https://game.creators-guild.com/gck2023/#main

## 制作概要
ゲームエンジン: Unity</br>
使用言語: C#</br>
使用ライブラリ: UniRx, UniTask, DoTween, InputSystem, Cinemachine etc...</br>
制作人数: プログラマ4～5名(途中制作メンバーが一人交代したため)</br>
製作期間: 約半年</br>

## ライブラリ概要
[UniRx](https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276?locale=ja-JP)はUnityのためのReactictive Extensionです。</br>
[UniTask](https://github.com/Cysharp/UniTask)はUnityに最適化された非同期処理を提供します。</br>
[DoTween](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676?locale=ja-JP)は主にUiアニメーションにまつわる機能を提供します。</br>
[InputSystem](https://docs.unity3d.com/ja/Packages/com.unity.inputsystem@1.4/manual/index.html)はInputのインタフェースを統合し、デバイスを気にせずInputを取得できる機能を提供します。</br>
[Cinemachine](https://unity.com/ja/unity/features/editor/art-and-design/cinemachine)は様々なカメラの機能を有するコンポーネントを提供します。</br>
尚、InputSystemとCinemachineについてはUnity公式より提供されています。</br>
※詳細はリンク先より参照いただければ幸いです。</br>

## コード概要
以下ファイル内のソースコードがリポジトリオーナーが作成したソースコードです。
> ChronuShifterDemo/Assets/_Chronus/snsk
> 
# ステートマシン
C#の機能のみを使用した、ピュアなステートマシンを作成し、それを用いてプレイヤーの挙動を制御しています。</br>
このステートマシンは階層型に対応しており、より柔軟にプレイヤーの挙動を制御できるようになっています。

# クリーンアーキテクチャ意識の設計
自身初の試みとして、**インゲーム部分にクリーンアーキテクチャを適用**しました。</br>
これによって、例えばステートマシンやステートの継承先クラスは完全なピュアC#クラスとして独立しているため、モジュールの細かい分割を可能にました。</br>
また、クリーンアーキテクチャを意識しているため、**ライブラリやUnityに依存方向が向かないように**設計されています。</br>
プレイヤーアニメーションについても、あくまで**キャラクターのViewである**と定義したかったため、主にUIの設計で用いられるMV(R)Pパターンを適用してみるなどの工夫も行いました。

# その他
コードには含まれていませんが、ShaderGraph(Unity内のノードベースShader作成ツール)を用いてディゾルブシェーダーを作成したり、タイムラインを一括で管理するクラスを作成したりなど、プレイヤー以外の細かい部分も担当しています。</br>
※なお、ShaderGraphはsource codeではないため本リポジトリには同梱されていません。

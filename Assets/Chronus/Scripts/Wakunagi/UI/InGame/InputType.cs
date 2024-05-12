namespace Chronus.UI.InGame {
    //Inputの区別
    public enum InputType {
        right, left, up, down,
        decision, open, back,
        end,
    }

    public class GetInputType {

        //スティックor十字キーの入力か
        public static bool IsInputArrow(InputType type) {
            if (type == InputType.right) return true;
            if (type == InputType.left) return true;
            if (type == InputType.up) return true;
            if (type == InputType.down) return true;
            return false;
        }

        //上下の入力
        public static int IsInputUpDown(InputType type) {
            if (type == InputType.up) return -1;    //上に行くほど若くなるため
            if (type == InputType.down) return 1;
            return 0;
        }

        //左右の入力
        public static int IsInputRightLeft(InputType type) {
            if (type == InputType.right) return 1;
            if (type == InputType.left) return -1;
            return 0;
        }

        public static bool IsInputDecision(InputType type) {
            return type == InputType.decision;
        }

        public static bool IsInputOpen(InputType type) {
            return type == InputType.open;
        }

        public static bool IsInputBack(InputType type) {
            return type == InputType.back;
        }
    }
}
namespace PrimitiveObsession
{
    public class Primitive<T>
    {
        public T Value { private get; set; }

        public static implicit operator T(Primitive<T> primitive) => 
            primitive.Value;
    }
}